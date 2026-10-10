const TOKEN_KEY = 'token'

export async function login(username: string, password: string): Promise<void> {
  let response: Response

  try {
    response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password }),
    })
  } catch {
    throw new Error("Can't reach the server. Please check it's running and try again.")
  }

  if (!response.ok) {
    throw new Error(await response.text())
  }

  const data: { token: string } = await response.json()
  sessionStorage.setItem(TOKEN_KEY, data.token)
}

export function getToken(): string | null {
  return sessionStorage.getItem(TOKEN_KEY)
}

export function logout(): void {
  sessionStorage.removeItem(TOKEN_KEY)
}