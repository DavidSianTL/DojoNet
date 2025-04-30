export interface User {
  id: string
  username: string
  password: string // In a real app, this would be hashed
  name: string
}

export interface Item {
  id: string
  name: string
  description: string
  category: string
  price: number
  createdAt: string
  updatedAt: string
}

export interface Session {
  userId: string
  username: string
  expiresAt: number
}
