"use client"

import { useEffect, useState } from "react"
import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"
import { PlusCircle, Edit, Trash2, LogOut, Database, Search, User, Package, RefreshCw } from "lucide-react"
import { checkSession, logout, getCurrentUser } from "@/lib/auth"
import { getItems, deleteItem } from "@/lib/items"
import type { Item } from "@/lib/types"
import ItemDialog from "@/components/item-dialog"
import { Input } from "@/components/ui/input"

export default function Dashboard() {
  const router = useRouter()
  const [items, setItems] = useState<Item[]>([])
  const [filteredItems, setFilteredItems] = useState<Item[]>([])
  const [loading, setLoading] = useState(true)
  const [dialogOpen, setDialogOpen] = useState(false)
  const [currentItem, setCurrentItem] = useState<Item | null>(null)
  const [searchTerm, setSearchTerm] = useState("")
  const [currentUser, setCurrentUser] = useState<any>(null)
  const [refreshing, setRefreshing] = useState(false)

  useEffect(() => {
    const validateSession = async () => {
      const isValid = await checkSession()
      if (!isValid) {
        router.push("/login")
      } else {
        setCurrentUser(getCurrentUser())
        loadItems()
      }
    }

    validateSession()
  }, [router])

  useEffect(() => {
    if (searchTerm.trim() === "") {
      setFilteredItems(items)
    } else {
      const filtered = items.filter(
        (item) =>
          item.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          item.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
          item.category.toLowerCase().includes(searchTerm.toLowerCase()),
      )
      setFilteredItems(filtered)
    }
  }, [searchTerm, items])

  const loadItems = async () => {
    try {
      const data = await getItems()
      setItems(data)
      setFilteredItems(data)
    } catch (error) {
      console.error("Failed to load items:", error)
    } finally {
      setLoading(false)
    }
  }

  const handleRefresh = async () => {
    setRefreshing(true)
    await loadItems()
    setTimeout(() => setRefreshing(false), 500)
  }

  const handleLogout = async () => {
    await logout()
    router.push("/login")
  }

  const handleAddNew = () => {
    setCurrentItem(null)
    setDialogOpen(true)
  }

  const handleEdit = (item: Item) => {
    setCurrentItem(item)
    setDialogOpen(true)
  }

  const handleDelete = async (id: string) => {
    if (window.confirm("Are you sure you want to delete this item?")) {
      try {
        await deleteItem(id)
        setItems(items.filter((item) => item.id !== id))
      } catch (error) {
        console.error("Failed to delete item:", error)
      }
    }
  }

  const handleDialogClose = (refresh: boolean) => {
    setDialogOpen(false)
    if (refresh) {
      loadItems()
    }
  }

  const getCategoryColor = (category: string) => {
    switch (category) {
      case "Electronics":
        return "bg-blue-100 text-blue-800 border-blue-200"
      case "Clothing":
        return "bg-purple-100 text-purple-800 border-purple-200"
      case "Books":
        return "bg-amber-100 text-amber-800 border-amber-200"
      case "Home":
        return "bg-green-100 text-green-800 border-green-200"
      default:
        return "bg-gray-100 text-gray-800 border-gray-200"
    }
  }

  if (loading) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
        <Database className="h-12 w-12 text-purple-600 animate-pulse mb-4" />
        <p className="text-lg text-gray-600">Loading your data...</p>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <header className="bg-white border-b border-gray-200 sticky top-0 z-10">
        <div className="max-w-7xl mx-auto py-4 px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center">
            <div className="flex items-center space-x-2">
              <Database className="h-8 w-8 text-purple-600" />
              <h1 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-purple-600 to-pink-600">
                DataManager
              </h1>
            </div>

            <div className="flex items-center space-x-4">
              <div className="flex items-center space-x-2 bg-gray-100 px-3 py-1.5 rounded-full">
                <User className="h-4 w-4 text-gray-500" />
                <span className="text-sm font-medium text-gray-700">{currentUser?.name || "User"}</span>
              </div>

              <Button
                variant="outline"
                onClick={handleLogout}
                className="border-gray-200 text-gray-700 hover:bg-gray-100 hover:text-gray-900"
              >
                <LogOut className="h-4 w-4 mr-2" />
                Logout
              </Button>
            </div>
          </div>
        </div>
      </header>

      <main className="flex-grow container mx-auto py-8 px-4">
        <div className="grid gap-6">
          <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
            <div>
              <h2 className="text-2xl font-bold text-gray-900">Items Management</h2>
              <p className="text-gray-500">Manage your inventory items</p>
            </div>

            <div className="flex flex-col sm:flex-row gap-3">
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Search className="h-4 w-4 text-gray-400" />
                </div>
                <Input
                  type="text"
                  placeholder="Search items..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="pl-10 w-full sm:w-64 bg-white border-gray-200"
                />
              </div>

              <div className="flex gap-2">
                <Button
                  variant="outline"
                  onClick={handleRefresh}
                  className="border-gray-200 text-gray-700 hover:bg-gray-100"
                  disabled={refreshing}
                >
                  <RefreshCw className={`h-4 w-4 mr-2 ${refreshing ? "animate-spin" : ""}`} />
                  Refresh
                </Button>

                <Button
                  onClick={handleAddNew}
                  className="bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 transition-all duration-300"
                >
                  <PlusCircle className="h-4 w-4 mr-2" />
                  Add New Item
                </Button>
              </div>
            </div>
          </div>

          <Card className="border-0 shadow-lg overflow-hidden">
            <CardHeader className="bg-gray-50 border-b border-gray-100 py-4">
              <div className="flex items-center space-x-2">
                <Package className="h-5 w-5 text-purple-600" />
                <CardTitle className="text-lg font-medium">Inventory Items</CardTitle>
              </div>
            </CardHeader>

            <CardContent className="p-0">
              {filteredItems.length === 0 ? (
                <div className="flex flex-col items-center justify-center py-12 px-4">
                  <div className="bg-gray-100 p-4 rounded-full mb-4">
                    <Package className="h-8 w-8 text-gray-400" />
                  </div>
                  <p className="text-gray-500 text-lg mb-2">No items found</p>
                  <p className="text-gray-400 text-center max-w-md mb-6">
                    {searchTerm
                      ? "Try adjusting your search terms or"
                      : "Get started by adding your first item to the inventory"}
                  </p>
                  {searchTerm && (
                    <Button variant="outline" onClick={() => setSearchTerm("")} className="mr-2">
                      Clear Search
                    </Button>
                  )}
                  <Button onClick={handleAddNew}>
                    <PlusCircle className="h-4 w-4 mr-2" />
                    Add New Item
                  </Button>
                </div>
              ) : (
                <div className="overflow-x-auto">
                  <Table>
                    <TableHeader>
                      <TableRow className="bg-gray-50 hover:bg-gray-50">
                        <TableHead className="font-medium">ID</TableHead>
                        <TableHead className="font-medium">Name</TableHead>
                        <TableHead className="font-medium">Description</TableHead>
                        <TableHead className="font-medium">Category</TableHead>
                        <TableHead className="font-medium">Price</TableHead>
                        <TableHead className="text-right font-medium">Actions</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {filteredItems.map((item) => (
                        <TableRow key={item.id} className="hover:bg-gray-50 transition-colors">
                          <TableCell className="font-medium text-gray-500">{item.id.substring(0, 8)}</TableCell>
                          <TableCell className="font-medium">{item.name}</TableCell>
                          <TableCell className="text-gray-600 max-w-xs truncate">{item.description}</TableCell>
                          <TableCell>
                            <Badge variant="outline" className={`${getCategoryColor(item.category)}`}>
                              {item.category}
                            </Badge>
                          </TableCell>
                          <TableCell className="font-medium">${item.price.toFixed(2)}</TableCell>
                          <TableCell className="text-right">
                            <div className="flex justify-end gap-2">
                              <Button
                                variant="outline"
                                size="sm"
                                onClick={() => handleEdit(item)}
                                className="h-8 border-gray-200 text-gray-700 hover:bg-gray-100 hover:text-gray-900"
                              >
                                <Edit className="h-3.5 w-3.5 mr-1" />
                                Edit
                              </Button>
                              <Button
                                variant="outline"
                                size="sm"
                                onClick={() => handleDelete(item.id)}
                                className="h-8 border-gray-200 text-red-600 hover:bg-red-50 hover:text-red-700 hover:border-red-200"
                              >
                                <Trash2 className="h-3.5 w-3.5 mr-1" />
                                Delete
                              </Button>
                            </div>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </main>

      {dialogOpen && <ItemDialog item={currentItem} open={dialogOpen} onClose={handleDialogClose} />}
    </div>
  )
}
