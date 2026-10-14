<template>
  <div class="products-page">
    <div class="container">
      <!-- Page Heading & Meta -->
      <header class="catalog-header">
        <div class="catalog-heading-text">
          <div class="catalog-eyebrow">
            <span class="eyebrow-dot"></span>
            <span>NOVA ARCHIVAL CATALOGUE</span>
          </div>
          <h1 class="catalog-title">
            Curated Products<span class="dot-accent">.</span>
          </h1>
          <p class="catalog-subtitle">
            Everyday design objects, modern audio, and high-performance essentials.
          </p>
        </div>

        <div class="catalog-meta-badge">
          <span class="count-number">{{ catalogStore.totalCount }}</span>
          <span class="count-label">Available Items</span>
        </div>
      </header>

      <!-- Main Filter & Search Control Toolbar -->
      <div class="catalog-toolbar glass-panel">
        <!-- Search Input -->
        <div class="search-input-box">
          <svg class="search-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"></circle>
            <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
          </svg>
          <input 
            v-model="searchQuery" 
            placeholder="Search by name, specs, or category..." 
            @input="scheduleSearch"
            aria-label="Search products"
          />
          <button v-if="searchQuery" class="clear-search-btn" @click="clearSearch" aria-label="Clear search">✕</button>
        </div>

        <!-- Sort Select -->
        <div class="sort-select-box">
          <label for="catalog-sort" class="sr-only">Sort by</label>
          <select id="catalog-sort" v-model="sortBy" @change="resetAndLoad">
            <option value="popular">⚡ Most Popular</option>
            <option value="price-asc">Price: Low to High</option>
            <option value="price-desc">Price: High to Low</option>
            <option value="rating">★ Highest Rated</option>
            <option value="new">✦ Newest Arrivals</option>
          </select>
          <span class="select-chevron">▾</span>
        </div>

        <!-- View Mode Switcher -->
        <button class="view-mode-pill" type="button" @click="toggleViewMode">
          <svg v-if="viewMode === 'infinite'" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <rect x="3" y="3" width="7" height="7"></rect>
            <rect x="14" y="3" width="7" height="7"></rect>
            <rect x="14" y="14" width="7" height="7"></rect>
            <rect x="3" y="14" width="7" height="7"></rect>
          </svg>
          <svg v-else width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <polyline points="19 12 12 19 5 12"></polyline>
          </svg>
          <span>{{ viewMode === 'infinite' ? 'Switch to Pages' : 'Infinite Scroll' }}</span>
        </button>
      </div>

      <!-- Category Filter Pills Bar -->
      <div class="category-pills-scroll" role="navigation" aria-label="Categories">
        <button 
          class="cat-filter-pill" 
          :class="{ 'active': selectedCategory === null }"
          @click="selectCategory(null)"
        >
          All Items
        </button>
        <button 
          v-for="category in catalogStore.categories" 
          :key="category.id" 
          class="cat-filter-pill"
          :class="{ 'active': selectedCategory === category.id }"
          @click="selectCategory(category.id)"
        >
          {{ category.name }}
        </button>
      </div>

      <!-- Active Filters Summary (if active) -->
      <div v-if="selectedCategory !== null || searchQuery" class="active-filter-indicators">
        <span class="active-label">Active filters:</span>
        <span v-if="selectedCategory !== null" class="filter-chip">
          Category: {{ getCategoryName(selectedCategory) }}
          <button @click="selectCategory(null)" aria-label="Remove category filter">✕</button>
        </span>
        <span v-if="searchQuery" class="filter-chip">
          "{{ searchQuery }}"
          <button @click="clearSearch" aria-label="Remove search filter">✕</button>
        </span>
        <button class="reset-all-btn" @click="resetAllFilters">Reset all</button>
      </div>

      <!-- Main Products Grid -->
      <div class="catalog-grid-container">
        <!-- Products Grid -->
        <div v-if="catalogStore.products.length > 0" class="products-grid">
          <article 
            v-for="product in catalogStore.products" 
            :key="product.id" 
            class="product-card glass-card"
          >
            <div class="product-media-container">
              <!-- Badges -->
              <div class="product-badge-group">
                <span v-if="product.discountPercent > 0" class="badge-pill badge-discount">
                  -{{ product.discountPercent }}%
                </span>
                <span v-else-if="product.isNew" class="badge-pill badge-lime">
                  New
                </span>
              </div>

              <!-- Wishlist Action -->
              <button 
                class="wishlist-toggle-btn"
                :class="{ 'active': isWishlisted(product.id) }"
                @click="toggleWishlist(product)"
                :aria-label="`Save ${product.name} to wishlist`"
              >
                <svg width="17" height="17" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
                </svg>
              </button>

              <router-link :to="`/products/${product.id}`" class="product-image-link">
                <img :src="productImage(product)" :alt="product.name" loading="lazy" />
                <span class="quick-view-overlay">View Details →</span>
              </router-link>
            </div>

            <div class="product-card-body">
              <div class="product-top-meta">
                <span class="product-cat-tag">{{ product.categoryName || 'Catalogue' }}</span>
                <span class="product-rating-tag">★ {{ product.rating.toFixed(1) }}</span>
              </div>

              <h2 class="product-title">
                <router-link :to="`/products/${product.id}`">{{ product.name }}</router-link>
              </h2>

              <div class="product-pricing-row">
                <div class="price-box">
                  <span class="current-price">₹{{ product.price.toLocaleString('en-IN') }}</span>
                  <span v-if="product.comparePrice" class="compare-price">₹{{ product.comparePrice.toLocaleString('en-IN') }}</span>
                </div>
                <span v-if="product.inStock" class="stock-pill in-stock">In Stock</span>
                <span v-else class="stock-pill out-stock">Sold Out</span>
              </div>

              <button 
                class="add-cart-action-btn"
                :class="{ 'is-adding': cartStore.isAdding(product.id) }"
                @click="addToCart(product)"
                :disabled="!product.inStock || cartStore.isAdding(product.id)"
              >
                <span v-if="cartStore.isAdding(product.id)" class="loading-spin"></span>
                <span v-else-if="!product.inStock">Out of Stock</span>
                <span v-else class="btn-inner">
                  <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                    <line x1="12" y1="5" x2="12" y2="19"></line>
                    <line x1="5" y1="12" x2="19" y2="12"></line>
                  </svg>
                  <span>Add to Bag</span>
                </span>
              </button>
            </div>
          </article>
        </div>

        <!-- Skeleton Loading Grid -->
        <div v-else-if="catalogStore.loading" class="products-grid">
          <div v-for="n in 12" :key="n" class="product-card skeleton-shimmer" style="height: 420px;"></div>
        </div>

        <!-- Empty State -->
        <div v-else-if="!catalogStore.loading && catalogStore.products.length === 0" class="empty-results-box glass-panel">
          <div class="empty-icon-circle">⌕</div>
          <h3>No matching items found</h3>
          <p>We couldn't find any products matching your current filters or search term.</p>
          <button class="btn-pill btn-pill-primary" @click="resetAllFilters">Reset all filters</button>
        </div>

        <!-- Error State -->
        <div v-else-if="catalogStore.error" class="catalog-error-box glass-panel">
          <span class="error-msg">{{ catalogStore.error }}</span>
          <button class="btn-pill btn-pill-outline" @click="resetAndLoad">Try Again</button>
        </div>

        <!-- Pagination / Infinite Scroll Footer -->
        <div class="catalog-footer-controls" v-if="catalogStore.totalPages > 0">
          <!-- Pagination Mode -->
          <div v-if="viewMode === 'pagination'" class="pagination-bar" aria-label="Pagination">
            <button 
              class="page-nav-btn" 
              @click="prevPage" 
              :disabled="currentPage === 1 || catalogStore.loading"
            >
              ← Previous
            </button>
            <div class="page-numbers-info">
              Page <strong>{{ currentPage }}</strong> of {{ catalogStore.totalPages }}
            </div>
            <button 
              class="page-nav-btn" 
              @click="nextPage" 
              :disabled="currentPage === catalogStore.totalPages || catalogStore.loading"
            >
              Next →
            </button>
          </div>

          <!-- Infinite Scroll Indicator -->
          <div v-else class="infinite-status">
            <div v-if="catalogStore.loading" class="infinite-loading-pill">
              <span class="loading-spin"></span>
              <span>Loading more essentials…</span>
            </div>
            <div v-else-if="currentPage >= catalogStore.totalPages" class="end-of-collection">
              <span>✦ You've reached the end of the collection ({{ catalogStore.totalCount }} items) ✦</span>
            </div>
            <div v-else class="scroll-more-hint">
              <span>Scroll down to discover more drops ↓</span>
            </div>
          </div>

          <!-- Invisible marker for intersection observer -->
          <div ref="loadMoreMarker" class="infinite-load-marker" aria-hidden="true"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import { useWishlistStore } from '../stores/wishlist'
import { useAuthStore } from '../stores/auth'
import { productImage } from '../utils/productImage'
import type { Product } from '../types'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()
const wishlistStore = useWishlistStore()
const authStore = useAuthStore()
const route = useRoute()

const initialCategory = Number(route.query.categoryId)
const selectedCategory = ref<number | null>(Number.isFinite(initialCategory) && initialCategory > 0 ? initialCategory : null)
const searchQuery = ref('')
const sortBy = ref('popular')
const currentPage = ref(1)
const viewMode = ref<'infinite' | 'pagination'>('infinite')
const loadMoreMarker = ref<HTMLElement | null>(null)
let observer: IntersectionObserver | undefined
let searchTimer: ReturnType<typeof setTimeout> | undefined

function loadProducts() {
  return catalogStore.fetchProducts({
    page: currentPage.value,
    pageSize: 24,
    categoryId: selectedCategory.value || undefined,
    search: searchQuery.value || undefined,
    sort: sortBy.value
  })
}

function resetAndLoad() {
  currentPage.value = 1
  catalogStore.clearError()
  return loadProducts()
}

function scheduleSearch() {
  if (searchTimer) clearTimeout(searchTimer)
  searchTimer = setTimeout(resetAndLoad, 300)
}

function clearSearch() {
  searchQuery.value = ''
  resetAndLoad()
}

function selectCategory(catId: number | null) {
  selectedCategory.value = catId
  resetAndLoad()
}

function getCategoryName(catId: number | null) {
  const found = catalogStore.categories.find(c => c.id === catId)
  return found ? found.name : 'All'
}

function resetAllFilters() {
  selectedCategory.value = null
  searchQuery.value = ''
  sortBy.value = 'popular'
  resetAndLoad()
}

function isWishlisted(productId: number) {
  return wishlistStore.items.some((item: any) => item.productId === productId)
}

function toggleWishlist(product: Product) {
  if (!authStore.isAuthenticated) {
    window.location.href = '/login'
    return
  }
  wishlistStore.toggleWishlist(product.id)
}

function addToCart(product: Product) {
  cartStore.addToCart(product.id, 1, undefined, product.name)
}

async function toggleViewMode() {
  viewMode.value = viewMode.value === 'infinite' ? 'pagination' : 'infinite'
  if (viewMode.value === 'pagination') {
    observer?.disconnect()
    await resetAndLoad()
  } else {
    observeMarker()
  }
}

function prevPage() {
  if (currentPage.value > 1) {
    const page = currentPage.value - 1
    loadProductsForPage(page)
  }
}

function nextPage() {
  if (currentPage.value < catalogStore.totalPages) {
    const page = currentPage.value + 1
    loadProductsForPage(page)
  }
}

async function loadProductsForPage(page: number) {
  currentPage.value = page
  try {
    await loadProducts()
    window.scrollTo({ top: 200, behavior: 'smooth' })
  } catch {
    currentPage.value = catalogStore.currentPage
  }
}

async function loadMore() {
  if (viewMode.value !== 'infinite' || currentPage.value >= catalogStore.totalPages || catalogStore.loading || catalogStore.error) return
  const nextPage = currentPage.value + 1
  const loaded = await catalogStore.appendProducts({
    page: nextPage,
    pageSize: 24,
    categoryId: selectedCategory.value || undefined,
    search: searchQuery.value || undefined,
    sort: sortBy.value
  })
  if (loaded) currentPage.value = nextPage
}

function observeMarker() {
  if (loadMoreMarker.value && observer) {
    observer.observe(loadMoreMarker.value)
  }
}

onMounted(() => {
  catalogStore.fetchCategories()
  loadProducts()
  observer = new IntersectionObserver(([entry]) => {
    if (viewMode.value === 'infinite' && entry.isIntersecting && !catalogStore.loading) {
      loadMore()
    }
  }, { rootMargin: '500px' })
  observeMarker()
})

watch(loadMoreMarker, observeMarker)
onBeforeUnmount(() => observer?.disconnect())
</script>

<style scoped>
.products-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

/* Catalog Header */
.catalog-header {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 2.5rem;
  gap: 2rem;
}

.catalog-eyebrow {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.72rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.8rem;
}

.eyebrow-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--accent-lime);
}

.catalog-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.8rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.catalog-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
  max-width: 500px;
}

.catalog-meta-badge {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  padding: 1rem 1.6rem;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-lg);
}

.count-number {
  font-family: var(--font-display);
  font-size: 2.2rem;
  font-weight: 900;
  line-height: 1;
  color: #ffffff;
}

.count-label {
  font-size: 0.75rem;
  font-weight: 600;
  color: var(--text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

/* Toolbar */
.catalog-toolbar {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.85rem 1.2rem;
  margin-bottom: 1.5rem;
  border-radius: var(--radius-lg);
}

.search-input-box {
  position: relative;
  flex: 1;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  left: 1rem;
  color: var(--text-muted);
  pointer-events: none;
}

.search-input-box input {
  width: 100%;
  padding: 0.75rem 2.5rem 0.75rem 2.8rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-pill);
  color: #ffffff;
  font-size: 0.88rem;
  outline: none;
  transition: all var(--transition-fast);
}

.search-input-box input:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
  box-shadow: 0 0 15px rgba(168, 85, 247, 0.25);
}

.clear-search-btn {
  position: absolute;
  right: 0.85rem;
  background: rgba(255, 255, 255, 0.1);
  border: 0;
  color: #ffffff;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  font-size: 0.75rem;
  cursor: pointer;
}

.sort-select-box {
  position: relative;
  min-width: 180px;
}

.sort-select-box select {
  width: 100%;
  appearance: none;
  padding: 0.75rem 2.2rem 0.75rem 1.2rem;
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-pill);
  color: #ffffff;
  font-size: 0.84rem;
  font-weight: 600;
  outline: none;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.sort-select-box select:focus {
  border-color: var(--accent-purple-bright);
}

.sort-select-box select option {
  background: var(--bg-surface);
  color: #ffffff;
}

.select-chevron {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: var(--text-muted);
  pointer-events: none;
}

.view-mode-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.2rem;
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-pill);
  color: #ffffff;
  font-size: 0.82rem;
  font-weight: 600;
  cursor: pointer;
  white-space: nowrap;
  transition: all var(--transition-fast);
}

.view-mode-pill:hover {
  background: rgba(255, 255, 255, 0.12);
  border-color: rgba(255, 255, 255, 0.25);
}

/* Category Filter Pills Scroll */
.category-pills-scroll {
  display: flex;
  gap: 0.6rem;
  overflow-x: auto;
  padding-bottom: 0.5rem;
  margin-bottom: 2rem;
  scrollbar-width: none;
}

.category-pills-scroll::-webkit-scrollbar {
  display: none;
}

.cat-filter-pill {
  padding: 0.5rem 1.1rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  font-size: 0.82rem;
  font-weight: 600;
  cursor: pointer;
  white-space: nowrap;
  transition: all var(--transition-fast);
}

.cat-filter-pill:hover {
  background: rgba(255, 255, 255, 0.08);
  color: #ffffff;
}

.cat-filter-pill.active {
  background: #ffffff;
  color: #0a0a0f;
  border-color: #ffffff;
  font-weight: 700;
  box-shadow: 0 4px 15px rgba(255, 255, 255, 0.25);
}

/* Active Filter Chips */
.active-filter-indicators {
  display: flex;
  align-items: center;
  gap: 0.65rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.active-label {
  font-size: 0.78rem;
  color: var(--text-muted);
}

.filter-chip {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.35rem 0.85rem;
  border-radius: var(--radius-pill);
  background: rgba(168, 85, 247, 0.15);
  border: 1px solid rgba(168, 85, 247, 0.35);
  color: #c084fc;
  font-size: 0.78rem;
  font-weight: 600;
}

.filter-chip button {
  background: transparent;
  border: 0;
  color: inherit;
  cursor: pointer;
  font-size: 0.85rem;
}

.reset-all-btn {
  background: transparent;
  border: 0;
  color: var(--text-muted);
  text-decoration: underline;
  font-size: 0.78rem;
  cursor: pointer;
}

.reset-all-btn:hover {
  color: #ffffff;
}

/* Products Grid */
.products-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.5rem;
}

.product-card {
  border-radius: var(--radius-lg);
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.product-media-container {
  position: relative;
  width: 100%;
  aspect-ratio: 1;
  background: #0d0c14;
  overflow: hidden;
}

.product-badge-group {
  position: absolute;
  top: 1rem;
  left: 1rem;
  z-index: 2;
  display: flex;
  gap: 0.4rem;
}

.wishlist-toggle-btn {
  position: absolute;
  top: 1rem;
  right: 1rem;
  z-index: 2;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(18, 17, 27, 0.7);
  backdrop-filter: blur(10px);
  border: 1px solid var(--border-subtle);
  color: rgba(255, 255, 255, 0.7);
  display: grid;
  place-items: center;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.wishlist-toggle-btn:hover,
.wishlist-toggle-btn.active {
  background: rgba(236, 72, 153, 0.2);
  color: var(--accent-pink);
  border-color: rgba(236, 72, 153, 0.4);
  transform: scale(1.08);
}

.product-image-link {
  display: block;
  width: 100%;
  height: 100%;
  position: relative;
}

.product-image-link img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--transition-smooth);
}

.product-card:hover .product-image-link img {
  transform: scale(1.08);
}

.quick-view-overlay {
  position: absolute;
  bottom: 1rem;
  left: 50%;
  transform: translateX(-50%) translateY(10px);
  padding: 0.45rem 1rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.9);
  color: #0a0a0f;
  font-size: 0.75rem;
  font-weight: 700;
  opacity: 0;
  transition: all var(--transition-fast);
  white-space: nowrap;
}

.product-card:hover .quick-view-overlay {
  opacity: 1;
  transform: translateX(-50%) translateY(0);
}

.product-card-body {
  padding: 1.4rem;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.product-top-meta {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.5rem;
}

.product-cat-tag {
  color: var(--text-muted);
  font-size: 0.72rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.06em;
}

.product-rating-tag {
  color: #fbbf24;
  font-size: 0.75rem;
  font-weight: 700;
}

.product-title {
  font-size: 1.05rem;
  font-weight: 700;
  margin-bottom: 1rem;
  line-height: 1.35;
  letter-spacing: -0.01em;
}

.product-title a {
  color: #ffffff;
  text-decoration: none;
  transition: color var(--transition-fast);
}

.product-title a:hover {
  color: var(--accent-purple-bright);
}

.product-pricing-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.2rem;
}

.price-box {
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
}

.current-price {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
}

.compare-price {
  font-size: 0.85rem;
  color: var(--text-muted);
  text-decoration: line-through;
}

.stock-pill {
  font-size: 0.7rem;
  font-weight: 700;
  padding: 0.2rem 0.55rem;
  border-radius: var(--radius-pill);
}

.stock-pill.in-stock {
  color: #4ade80;
  background: rgba(34, 197, 94, 0.12);
}

.stock-pill.out-stock {
  color: #f87171;
  background: rgba(239, 68, 68, 0.12);
}

.add-cart-action-btn {
  width: 100%;
  padding: 0.75rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-size: 0.84rem;
  font-weight: 700;
  cursor: pointer;
  transition: all var(--transition-fast);
  margin-top: auto;
}

.add-cart-action-btn:hover:not(:disabled) {
  background: #ffffff;
  color: #0a0a0f;
  border-color: #ffffff;
  box-shadow: 0 4px 20px rgba(255, 255, 255, 0.25);
  transform: translateY(-2px);
}

.add-cart-action-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.btn-inner {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.loading-spin {
  display: inline-block;
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: #ffffff;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

/* Empty State */
.empty-results-box {
  padding: 5rem 2rem;
  text-align: center;
  max-width: 500px;
  margin: 3rem auto;
}

.empty-icon-circle {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.05);
  font-size: 1.8rem;
  display: grid;
  place-items: center;
  margin: 0 auto 1.5rem;
  color: var(--accent-purple-bright);
}

.empty-results-box h3 {
  font-size: 1.4rem;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 0.6rem;
}

.empty-results-box p {
  color: var(--text-secondary);
  font-size: 0.92rem;
  margin-bottom: 1.8rem;
}

/* Error Box */
.catalog-error-box {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1.5rem;
  padding: 2rem;
  margin: 3rem 0;
  border-color: rgba(239, 68, 68, 0.3);
}

.error-msg {
  color: #fca5a5;
  font-size: 0.92rem;
}

/* Footer Controls */
.catalog-footer-controls {
  margin-top: 4rem;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.pagination-bar {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.page-nav-btn {
  padding: 0.75rem 1.4rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-size: 0.84rem;
  font-weight: 700;
  cursor: pointer;
  transition: all var(--transition-fast);
}

.page-nav-btn:hover:not(:disabled) {
  background: #ffffff;
  color: #0a0a0f;
  transform: translateY(-2px);
}

.page-nav-btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.page-numbers-info {
  font-size: 0.88rem;
  color: var(--text-secondary);
}

.page-numbers-info strong {
  color: #ffffff;
}

.infinite-status {
  text-align: center;
}

.infinite-loading-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1.4rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  font-size: 0.84rem;
  font-weight: 600;
}

.end-of-collection {
  color: var(--text-muted);
  font-size: 0.82rem;
  letter-spacing: 0.05em;
}

.scroll-more-hint {
  color: var(--text-muted);
  font-size: 0.82rem;
}

.infinite-load-marker {
  height: 10px;
}

/* Responsive Grid */
@media (max-width: 1200px) {
  .products-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (max-width: 900px) {
  .catalog-header {
    flex-direction: column;
    align-items: flex-start;
  }
  .catalog-meta-badge {
    align-self: flex-start;
  }
  .catalog-toolbar {
    flex-wrap: wrap;
  }
  .sort-select-box {
    flex: 1;
  }
  .products-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 600px) {
  .catalog-title {
    font-size: 2.8rem;
  }
  .search-input-box {
    width: 100%;
  }
  .sort-select-box {
    width: 100%;
  }
  .view-mode-pill {
    width: 100%;
    justify-content: center;
  }
  .products-grid {
    grid-template-columns: 1fr;
  }
}
</style>
