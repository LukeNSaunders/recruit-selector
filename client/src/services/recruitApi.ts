import type { RecruitResult } from '../types/recruit'

export async function selectRecruits(recruits: number, skip: number): Promise<RecruitResult> {
  let response: Response

  try {
    response = await fetch(`/api/recruits?recruits=${recruits}&skip=${skip}`)
  } catch {
    throw new Error("Can't reach the server. Please check it's running and try again.")
  }

  if (!response.ok) {
    throw new Error(await response.text())
  }

  return await response.json()
}