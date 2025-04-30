import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { ArrowRight, Database, Lock, FileText, Paintbrush, CheckCircle } from "lucide-react"

export default function Home() {
  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-gray-100 flex flex-col">
      <header className="bg-white border-b border-gray-200">
        <div className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8 flex justify-between items-center">
          <div className="flex items-center space-x-2">
            <Database className="h-8 w-8 text-purple-600" />
            <h1 className="text-3xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-purple-600 to-pink-600">
              DataManager
            </h1>
          </div>
          <Link href="/login">
            <Button className="bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 transition-all duration-300 shadow-md hover:shadow-lg">
              Login
            </Button>
          </Link>
        </div>
      </header>

      <main className="flex-grow flex flex-col items-center justify-center p-6">
        <div className="max-w-6xl w-full mx-auto grid gap-8 md:grid-cols-2">
          <div className="flex flex-col justify-center space-y-6">
            <h2 className="text-4xl md:text-5xl font-bold leading-tight text-gray-900">
              Powerful CRUD Application with{" "}
              <span className="bg-clip-text text-transparent bg-gradient-to-r from-purple-600 to-pink-600">
                Beautiful Design
              </span>
            </h2>
            <p className="text-xl text-gray-600">
              A complete data management solution with authentication, validation, and a stunning user interface.
            </p>
            <div className="pt-4">
              <Link href="/login">
                <Button
                  size="lg"
                  className="bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 transition-all duration-300 shadow-md hover:shadow-lg text-lg px-8 py-6 h-auto"
                >
                  Get Started <ArrowRight className="ml-2 h-5 w-5" />
                </Button>
              </Link>
            </div>
          </div>

          <Card className="bg-white shadow-xl border-0 overflow-hidden">
            <div className="absolute inset-0 bg-gradient-to-br from-purple-500/10 to-pink-500/10 rounded-lg" />
            <CardHeader className="relative">
              <CardTitle className="text-2xl">Features</CardTitle>
              <CardDescription>Everything you need for data management</CardDescription>
            </CardHeader>
            <CardContent className="relative space-y-4">
              <div className="flex items-start space-x-3">
                <div className="bg-purple-100 p-2 rounded-lg">
                  <Lock className="h-5 w-5 text-purple-600" />
                </div>
                <div>
                  <h3 className="font-medium">Secure Authentication</h3>
                  <p className="text-sm text-gray-500">User authentication with session management and validation</p>
                </div>
              </div>

              <div className="flex items-start space-x-3">
                <div className="bg-pink-100 p-2 rounded-lg">
                  <Database className="h-5 w-5 text-pink-600" />
                </div>
                <div>
                  <h3 className="font-medium">Complete CRUD Operations</h3>
                  <p className="text-sm text-gray-500">Create, read, update and delete with JSON persistence</p>
                </div>
              </div>

              <div className="flex items-start space-x-3">
                <div className="bg-purple-100 p-2 rounded-lg">
                  <CheckCircle className="h-5 w-5 text-purple-600" />
                </div>
                <div>
                  <h3 className="font-medium">Form Validation</h3>
                  <p className="text-sm text-gray-500">Client-side and server-side validation for data integrity</p>
                </div>
              </div>

              <div className="flex items-start space-x-3">
                <div className="bg-pink-100 p-2 rounded-lg">
                  <Paintbrush className="h-5 w-5 text-pink-600" />
                </div>
                <div>
                  <h3 className="font-medium">Beautiful UI Design</h3>
                  <p className="text-sm text-gray-500">Modern, responsive interface with custom styling</p>
                </div>
              </div>

              <div className="flex items-start space-x-3">
                <div className="bg-purple-100 p-2 rounded-lg">
                  <FileText className="h-5 w-5 text-purple-600" />
                </div>
                <div>
                  <h3 className="font-medium">Session Management</h3>
                  <p className="text-sm text-gray-500">Secure session validation on protected routes</p>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      </main>

      <footer className="bg-white border-t border-gray-200 mt-auto">
        <div className="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
          <p className="text-center text-gray-500 text-sm">
            &copy; {new Date().getFullYear()} DataManager CRUD Application
          </p>
        </div>
      </footer>
    </div>
  )
}
