"use client"

import type { User, Session } from "./types"

// In a real application, this would be stored in a database
const USERS: User[] = [
  {
    id: "1",
    username: "admin",
    password: "admin123", // In a real app, this would be hashed
    name: "Administrator",
  },
]

// Session management
const SESSION_KEY = "app_session"

export async function login(username: string, password: string): Promise<boolean> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 500))

  const user = USERS.find((u) => u.username.toLowerCase() === username.toLowerCase() && u.password === password)

  if (user) {
    // Create session
    const session: Session = {
      userId: user.id,
      username: user.username,
      expiresAt: Date.now() + 24 * 60 * 60 * 1000, // 24 hours
    }

    // Store session in localStorage
    localStorage.setItem(SESSION_KEY, JSON.stringify(session))
    return true
  }

  return false
}

export async function logout(): Promise<void> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 300))

  // Remove session
  localStorage.removeItem(SESSION_KEY)
}

export async function checkSession(): Promise<boolean> {
  // Simulate API call delay
  await new Promise((resolve) => setTimeout(resolve, 300))

  const sessionData = localStorage.getItem(SESSION_KEY)

  if (!sessionData) {
    return false
  }

  try {
    const session: Session = JSON.parse(sessionData)

    // Check if session is expired
    if (session.expiresAt < Date.now()) {
      localStorage.removeItem(SESSION_KEY)
      return false
    }

    // Extend session
    session.expiresAt = Date.now() + 24 * 60 * 60 * 1000
    localStorage.setItem(SESSION_KEY, JSON.stringify(session))

    return true
  } catch (error) {
    console.error("Invalid session data:", error)
    localStorage.removeItem(SESSION_KEY)
    return false
  }
}

export function getCurrentUser(): User | null {
  const sessionData = localStorage.getItem(SESSION_KEY)

  if (!sessionData) {
    return null
  }

  try {
    const session: Session = JSON.parse(sessionData)
    const user = USERS.find((u) => u.id === session.userId)
    return user || null
  } catch {
    return null
  }
}
