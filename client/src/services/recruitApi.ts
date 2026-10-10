import type { RecruitResult } from '../types/recruit'
import { getToken, logout } from './auth'

export async function selectRecruits(recruits: number, skip: number): Promise<RecruitResult> {
  let response: Response

  try {
    response = await fetch(`/api/recruits?recruits=${recruits}&skip=${skip}`, {
      headers: { Authorization: `Bearer ${getToken()}` },
    })
  } catch {
    throw new Error("Can't reach the server. Please check it's running and try again.")
  }

  if (response.status === 401) {
    logout()
  }

  if (!response.ok) {
    throw new Error(await response.text())
  }

  return await response.json()
}