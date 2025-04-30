"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { Badge } from "@/components/ui/badge"
import type { Item } from "@/lib/types"
import { createItem, updateItem } from "@/lib/items"
import { AlertCircle, Package, Tag, FileText, DollarSign } from "lucide-react"

interface ItemDialogProps {
  item: Item | null
  open: boolean
  onClose: (refresh: boolean) => void
}

export default function ItemDialog({ item, open, onClose }: ItemDialogProps) {
  const [formData, setFormData] = useState<Partial<Item>>({
    name: "",
    description: "",
    category: "",
    price: 0,
  })
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [loading, setLoading] = useState(false)
  const [generalError, setGeneralError] = useState("")

  useEffect(() => {
    if (item) {
      setFormData({
        name: item.name,
        description: item.description,
        category: item.category,
        price: item.price,
      })
    } else {
      setFormData({
        name: "",
        description: "",
        category: "",
        price: 0,
      })
    }
    setErrors({})
    setGeneralError("")
  }, [item])

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {}

    if (!formData.name?.trim()) {
      newErrors.name = "Name is required"
    }

    if (!formData.description?.trim()) {
      newErrors.description = "Description is required"
    }

    if (!formData.category?.trim()) {
      newErrors.category = "Category is required"
    }

    if (formData.price === undefined || formData.price < 0) {
      newErrors.price = "Price must be a positive number"
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = async () => {
    if (!validateForm()) {
      return
    }

    try {
      setLoading(true)

      if (item) {
        await updateItem(item.id, formData as Item)
      } else {
        await createItem(formData as Item)
      }

      onClose(true)
    } catch (error) {
      console.error("Failed to save item:", error)
      setGeneralError("Failed to save item. Please try again.")
    } finally {
      setLoading(false)
    }
  }

  const handleChange = (field: string, value: string | number) => {
    setFormData((prev) => ({ ...prev, [field]: value }))
    if (errors[field]) {
      setErrors((prev) => {
        const newErrors = { ...prev }
        delete newErrors[field]
        return newErrors
      })
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

  return (
    <Dialog open={open} onOpenChange={(open) => !open && onClose(false)}>
      <DialogContent className="sm:max-w-[550px] p-0 overflow-hidden">
        <div className="bg-gradient-to-r from-purple-600 to-pink-600 p-6">
          <div className="bg-white/20 w-12 h-12 rounded-full flex items-center justify-center mb-3">
            <Package className="h-6 w-6 text-white" />
          </div>
          <DialogHeader className="space-y-1 p-0">
            <DialogTitle className="text-2xl font-bold text-white">{item ? "Edit Item" : "Add New Item"}</DialogTitle>
            <p className="text-purple-100 font-light">
              {item ? "Update the details of your existing item" : "Add a new item to your inventory"}
            </p>
          </DialogHeader>
        </div>

        <div className="p-6">
          {generalError && (
            <Alert variant="destructive" className="mb-6 bg-red-50 border border-red-200">
              <AlertCircle className="h-4 w-4 mr-2 text-red-600" />
              <AlertDescription className="text-red-800">{generalError}</AlertDescription>
            </Alert>
          )}

          <div className="grid gap-5 py-2">
            <div className="grid gap-2">
              <div className="flex items-center">
                <Tag className="h-4 w-4 mr-2 text-gray-500" />
                <Label htmlFor="name" className="font-medium">
                  Item Name
                </Label>
              </div>
              <Input
                id="name"
                value={formData.name}
                onChange={(e) => handleChange("name", e.target.value)}
                className={`bg-gray-50 border-gray-200 focus:bg-white transition-colors ${errors.name ? "border-red-300 focus:border-red-500" : ""}`}
                placeholder="Enter item name"
              />
              {errors.name && <p className="text-red-500 text-sm">{errors.name}</p>}
            </div>

            <div className="grid gap-2">
              <div className="flex items-center">
                <FileText className="h-4 w-4 mr-2 text-gray-500" />
                <Label htmlFor="description" className="font-medium">
                  Description
                </Label>
              </div>
              <Textarea
                id="description"
                value={formData.description}
                onChange={(e) => handleChange("description", e.target.value)}
                className={`bg-gray-50 border-gray-200 focus:bg-white transition-colors min-h-[100px] ${errors.description ? "border-red-300 focus:border-red-500" : ""}`}
                placeholder="Enter item description"
              />
              {errors.description && <p className="text-red-500 text-sm">{errors.description}</p>}
            </div>

            <div className="grid gap-2">
              <div className="flex items-center">
                <Tag className="h-4 w-4 mr-2 text-gray-500" />
                <Label htmlFor="category" className="font-medium">
                  Category
                </Label>
              </div>
              <Select value={formData.category} onValueChange={(value) => handleChange("category", value)}>
                <SelectTrigger
                  className={`bg-gray-50 border-gray-200 focus:bg-white transition-colors ${errors.category ? "border-red-300 focus:border-red-500" : ""}`}
                >
                  <SelectValue placeholder="Select a category" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="Electronics">
                    <div className="flex items-center">
                      <Badge variant="outline" className="bg-blue-100 text-blue-800 border-blue-200 mr-2">
                        Electronics
                      </Badge>
                    </div>
                  </SelectItem>
                  <SelectItem value="Clothing">
                    <div className="flex items-center">
                      <Badge variant="outline" className="bg-purple-100 text-purple-800 border-purple-200 mr-2">
                        Clothing
                      </Badge>
                    </div>
                  </SelectItem>
                  <SelectItem value="Books">
                    <div className="flex items-center">
                      <Badge variant="outline" className="bg-amber-100 text-amber-800 border-amber-200 mr-2">
                        Books
                      </Badge>
                    </div>
                  </SelectItem>
                  <SelectItem value="Home">
                    <div className="flex items-center">
                      <Badge variant="outline" className="bg-green-100 text-green-800 border-green-200 mr-2">
                        Home
                      </Badge>
                    </div>
                  </SelectItem>
                  <SelectItem value="Other">
                    <div className="flex items-center">
                      <Badge variant="outline" className="bg-gray-100 text-gray-800 border-gray-200 mr-2">
                        Other
                      </Badge>
                    </div>
                  </SelectItem>
                </SelectContent>
              </Select>
              {errors.category && <p className="text-red-500 text-sm">{errors.category}</p>}
            </div>

            <div className="grid gap-2">
              <div className="flex items-center">
                <DollarSign className="h-4 w-4 mr-2 text-gray-500" />
                <Label htmlFor="price" className="font-medium">
                  Price
                </Label>
              </div>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <span className="text-gray-500">$</span>
                </div>
                <Input
                  id="price"
                  type="number"
                  min="0"
                  step="0.01"
                  value={formData.price}
                  onChange={(e) => handleChange("price", Number.parseFloat(e.target.value) || 0)}
                  className={`pl-8 bg-gray-50 border-gray-200 focus:bg-white transition-colors ${errors.price ? "border-red-300 focus:border-red-500" : ""}`}
                  placeholder="0.00"
                />
              </div>
              {errors.price && <p className="text-red-500 text-sm">{errors.price}</p>}
            </div>
          </div>
        </div>

        <DialogFooter className="bg-gray-50 p-4 border-t border-gray-100">
          <Button
            variant="outline"
            onClick={() => onClose(false)}
            className="border-gray-200 text-gray-700 hover:bg-gray-100"
          >
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={loading}
            className="bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 transition-all duration-300"
          >
            {loading ? "Saving..." : item ? "Update Item" : "Create Item"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
