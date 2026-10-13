import { test, expect } from '@playwright/test'

test('homepage loads', async ({ page }) => {
  await page.goto('http://localhost:5173')
  await expect(page.locator('h1')).toContainText('NovaKart')
})

test('products page loads', async ({ page }) => {
  await page.goto('http://localhost:5173/products')
  await expect(page.locator('h1')).toContainText('Products')
})

test('login page loads', async ({ page }) => {
  await page.goto('http://localhost:5173/login')
  await expect(page.locator('h1')).toContainText('Login')
})

test('register page loads', async ({ page }) => {
  await page.goto('http://localhost:5173/register')
  await expect(page.locator('h1')).toContainText('Register')
})

test('contact page loads', async ({ page }) => {
  await page.goto('http://localhost:5173/contact')
  await expect(page.locator('h1')).toContainText('Contact Us')
})
