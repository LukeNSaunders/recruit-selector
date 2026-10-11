import { defineConfig, devices } from '@playwright/test'
import { TEST_USERNAME, TEST_PASSWORD } from './e2e/testUser'

export default defineConfig({
  // Where the user tests live
  testDir: './e2e',

  use: {
    // The address of the Vue app
    baseURL: 'http://localhost:5173',

    // When SLOW is set, pause half a second between each action so you can watch
    launchOptions: {
      slowMo: process.env.SLOW ? 1000 : 0,
    },
  },

  // Run the tests in Chrome
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],

  // Start both halves of the app before the tests
  webServer: [
    {
      // The real C# server, with test-only login details
      command: 'dotnet run --project ../server',
      // The server is ready once this address answers (401 counts, it means it's up)
      url: 'http://localhost:5000/api/recruits',
      reuseExistingServer: false,
      timeout: 120_000,
      env: {
        Jwt__Key: 'e2e-test-signing-key-that-is-at-least-32-chars',
        Auth__Username: TEST_USERNAME,
        Auth__Password: TEST_PASSWORD,
      },
    },
    {
      // The Vue app
      command: 'npm run dev',
      url: 'http://localhost:5173',
      reuseExistingServer: true,
    },
  ],
})