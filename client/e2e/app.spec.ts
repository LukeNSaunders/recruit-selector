import { test, expect, type Page } from '@playwright/test'
import { TEST_USERNAME, TEST_PASSWORD } from './testUser'

test.beforeEach(async ({ page }) => {
  await page.goto('/')
})

async function logIn(page: Page, password = TEST_PASSWORD) {
  await page.locator('header').getByRole('button', { name: 'Log in' }).click()

  const loginBox = page.getByRole('dialog')
  await loginBox.getByLabel('Username').fill(TEST_USERNAME)
  await loginBox.getByLabel('Password').fill(password)
  await loginBox.getByRole('button', { name: 'Log in' }).click()
}

async function runSelection(page: Page, recruits: string, skip: string) {
  await page.getByLabel('Number of recruits').fill(recruits)
  await page.getByLabel('Recruits to skip').fill(skip)
  await page.getByRole('button', { name: 'Run selection' }).click()
}

test('the user sees the page with a log in button', async ({ page }) => {
  await expect(page.getByRole('heading', { name: 'Who Comes Back Next Year?' })).toBeVisible()
  await expect(page.locator('header').getByRole('button', { name: 'Log in' })).toBeVisible()
})

test('the user can open and close the login box', async ({ page }) => {
  await page.locator('header').getByRole('button', { name: 'Log in' }).click()
  await expect(page.getByRole('dialog')).toBeVisible()

  await page.getByRole('button', { name: 'Cancel' }).click()
  await expect(page.getByRole('dialog')).toBeHidden()
})

test('the user can log in', async ({ page }) => {
  await logIn(page)

  await expect(page.getByRole('dialog')).toBeHidden()
  await expect(page.getByText('Successfully logged in!')).toBeVisible()
  await expect(page.getByRole('button', { name: 'Log out' })).toBeVisible()
})

test('the user sees an error with the wrong password', async ({ page }) => {
  await logIn(page, 'wrong-password')

  const loginBox = page.getByRole('dialog')
  await expect(loginBox).toBeVisible()
  await expect(loginBox.getByText('Incorrect username or password.')).toBeVisible()
})

test('the user stays logged in after refreshing the page', async ({ page }) => {
  await logIn(page)
  await expect(page.getByRole('button', { name: 'Log out' })).toBeVisible()

  await page.reload()

  await expect(page.getByRole('button', { name: 'Log out' })).toBeVisible()
})

test('the user is asked to log in when running a selection logged out', async ({ page }) => {
  await page.getByRole('button', { name: 'Run selection' }).click()

  await expect(page.getByText('Please log in to continue.')).toBeVisible()
  await expect(page.getByRole('dialog')).toBeVisible()
})

test('the user can run a selection and see the results', async ({ page }) => {
  await logIn(page)
  await runSelection(page, '5', '1')

  await expect(page.locator('.selected-list li')).toHaveText(['2', '4', '1', '5'])
  await expect(page.locator('.final-number')).toHaveText('3')
})

test('the user gets a different answer for different numbers', async ({ page }) => {
  await logIn(page)
  await runSelection(page, '7', '2')

  await expect(page.locator('.selected-list li')).toHaveText(['3', '6', '2', '7', '5', '1'])
  await expect(page.locator('.final-number')).toHaveText('4')
})

test('the user sees a message when there is only one recruit', async ({ page }) => {
  await logIn(page)
  await runSelection(page, '1', '1')

  await expect(page.getByText('Only one recruit was entered, so no one was selected.')).toBeVisible()
  await expect(page.locator('.final-number')).toBeHidden()
})

test('the user can log out, which clears the results', async ({ page }) => {
  await logIn(page)
  await runSelection(page, '5', '1')
  await expect(page.locator('.final-number')).toBeVisible()

  await page.getByRole('button', { name: 'Log out' }).click()

  await expect(page.getByText('Successfully logged out!')).toBeVisible()
  await expect(page.locator('header').getByRole('button', { name: 'Log in' })).toBeVisible()
  await expect(page.locator('.final-number')).toBeHidden()
})