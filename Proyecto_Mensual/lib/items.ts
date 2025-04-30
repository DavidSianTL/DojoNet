"use client"

import type { Item } from "./types"

// In a real application, this would be stored in a database
// For this example, we'll use localStorage to persist data
const ITEMS_KEY = "app_items"

// Initialize with some sample data if empty
const initializeItems = (): void => {
  if (!localStorage.getItem(ITEMS_KEY)) {
    const sampleItems: Item[] = [
      {
        id: "1",
        name: "Laptop",
        description: "High-performance laptop with 16GB RAM",
        category: "Electronics",
        price: 1299.99,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      },
      {
        id: "2",
        name: "Desk Chair",
        description: "Ergonomic office chair with lumbar support",
        category: "Home",
        price: 249.99,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      },
    ]

    localStorage.setItem(ITEMS_KEY, JSON.stringify(sampleItems))
  }
}

// Get all items
export async function getItems(): Promise<Item[]> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 500))

  initializeItems()

  try {
    const items = localStorage.getItem(ITEMS_KEY)
    return items ? JSON.parse(items) : []
  } catch (error) {
    console.error("Failed to parse items:", error)
    return []
  }
}

// Get a single item by ID
export async function getItem(id: string): Promise<Item | null> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 300))

  try {
    const items: Item[] = JSON.parse(localStorage.getItem(ITEMS_KEY) || "[]")
    return items.find((item) => item.id === id) || null
  } catch (error) {
    console.error("Failed to get item:", error)
    return null
  }
}

// Create a new item
export async function createItem(itemData: Partial<Item>): Promise<Item> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 700))

  try {
    const items: Item[] = JSON.parse(localStorage.getItem(ITEMS_KEY) || "[]")

    const newItem: Item = {
      ...(itemData as Item),
      id: Math.random().toString(36).substring(2, 11),
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    }

    items.push(newItem)
    localStorage.setItem(ITEMS_KEY, JSON.stringify(items))

    return newItem
  } catch (error) {
    console.error("Failed to create item:", error)
    throw new Error("Failed to create item")
  }
}

// Update an existing item
export async function updateItem(id: string, itemData: Partial<Item>): Promise<Item> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 700))

  try {
    const items: Item[] = JSON.parse(localStorage.getItem(ITEMS_KEY) || "[]")
    const index = items.findIndex((item) => item.id === id)

    if (index === -1) {
      throw new Error("Item not found")
    }

    const updatedItem: Item = {
      ...items[index],
      ...itemData,
      updatedAt: new Date().toISOString(),
    }

    items[index] = updatedItem
    localStorage.setItem(ITEMS_KEY, JSON.stringify(items))

    return updatedItem
  } catch (error) {
    console.error("Failed to update item:", error)
    throw new Error("Failed to update item")
  }
}

// Delete an item
export async function deleteItem(id: string): Promise<void> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 500))

  try {
    const items: Item[] = JSON.parse(localStorage.getItem(ITEMS_KEY) || "[]")
    const filteredItems = items.filter((item) => item.id !== id)

    localStorage.setItem(ITEMS_KEY, JSON.stringify(filteredItems))
  } catch (error) {
    console.error("Failed to delete item:", error)
    throw new Error("Failed to delete item")
  }
}
