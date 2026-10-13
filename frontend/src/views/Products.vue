<template>
  <div class="products">
    <div class="page-heading">
      <div><span class="eyebrow">THE NOVAKART EDIT</span><h1>Shop the collection<span>.</span></h1><p>Everyday objects, selected with intention.</p></div>
      <span class="result-count">{{ catalogStore.totalCount }} products</span>
    </div>

    <div class="catalog-layout">
      <aside class="filter-rail">
        <div class="rail-title"><span>FILTERS</span><span>⌘</span></div>
        <label>Category<select v-model="selectedCategory" @change="resetAndLoad">
          <option :value="null">All categories</option>
          <option v-for="category in catalogStore.categories" :key="category.id" :value="category.id">{{ category.name }}</option>
        </select></label>
        <div class="rail-note"><strong>Curated for you</strong><span>New arrivals, bestsellers and everyday essentials.</span></div>
      </aside>
      <div class="catalog-results">
        <div class="filters">
      <div class="search-wrap"><span>⌕</span><input v-model="searchQuery" placeholder="Search products..." @input="scheduleSearch" /></div>

      <select v-model="sortBy" @change="resetAndLoad">
        <option value="popular">Popular</option>
        <option value="price-asc">Price: Low to High</option>
        <option value="price-desc">Price: High to Low</option>
        <option value="rating">Rating</option>
        <option value="new">Newest</option>
      </select>
      <button class="view-mode-button" type="button" @click="toggleViewMode">
        {{ viewMode === 'infinite' ? 'Use pagination' : 'Use infinite scroll' }}
      </button>
      </div>

    <div class="grid" v-if="catalogStore.products.length">
      <div v-for="product in catalogStore.products" :key="product.id" class="card">
        <router-link :to="`/products/${product.id}`" class="image-wrap">
          <img :src="productImage(product)" :alt="product.name" />
          <span v-if="product.discountPercent > 0" class="discount-badge">{{ product.discountPercent }}% OFF</span>
          <span class="quick-view">View details ↗</span>
        </router-link>
        <div class="card-body">
          <div class="card-topline"><p class="category">{{ product.categoryName }}</p><span class="rating">★ {{ product.rating.toFixed(1) }}</span></div>
          <h3>{{ product.name }}</h3>
          <div class="price-row"><p class="price">₹{{ product.price.toLocaleString('en-IN') }}</p><p v-if="product.comparePrice" class="compare-price">₹{{ product.comparePrice.toLocaleString('en-IN') }}</p></div>
          <button :class="{ 'is-adding': cartStore.isAdding(product.id) }" @click="addToCart(product)" :disabled="!product.inStock || cartStore.isAdding(product.id)"><span>{{ cartStore.isAdding(product.id) ? '…' : '+' }}</span>{{ cartStore.isAdding(product.id) ? 'Adding…' : product.inStock ? 'Add to cart' : 'Out of stock' }}</button>
        </div>
      </div>
    </div>
    <p v-else-if="catalogStore.loading" class="loading-state">Loading the collection...</p>
    <div v-else-if="catalogStore.error" class="catalog-error initial-error">
      <span>{{ catalogStore.error }}</span>
      <button @click="resetAndLoad">Retry</button>
    </div>

    <div class="catalog-footer" v-if="catalogStore.totalPages > 0">
      <div v-if="catalogStore.error" class="catalog-error">
        <span>{{ catalogStore.error }}</span>
        <button @click="retryLoadMore">Retry</button>
      </div>
      <span v-else-if="catalogStore.loading" class="loading-indicator"><i></i> Loading more products...</span>
      <span v-else-if="currentPage >= catalogStore.totalPages" class="end-indicator">You reached the end of the collection</span>
      <span v-else class="scroll-hint">Scroll for more products</span>
      <div v-if="viewMode === 'pagination'" class="pagination" aria-label="Product pagination">
        <button @click="prevPage" :disabled="currentPage === 1 || catalogStore.loading">← Previous</button>
        <span>Page <strong>{{ currentPage }}</strong> of {{ catalogStore.totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === catalogStore.totalPages || catalogStore.loading">Next →</button>
      </div>
      <span v-else class="scroll-hint">Infinite scroll is on · scroll to load more</span>
      <button v-if="viewMode === 'infinite' && currentPage < catalogStore.totalPages && catalogStore.error" class="load-more-button" @click="retryLoadMore">Retry loading</button>
      <div ref="loadMoreMarker" class="load-more-marker" aria-hidden="true"></div>
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
import { productImage } from '../utils/productImage'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()
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

function addToCart(product: typeof catalogStore.products[number]) {
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

async function retryLoadMore() {
  catalogStore.clearError()
  await loadMore()
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
    if (viewMode.value === 'infinite' && entry.isIntersecting && !catalogStore.loading) loadMore()
  }, { rootMargin: '500px' })
  observeMarker()
})

watch(loadMoreMarker, observeMarker)
onBeforeUnmount(() => observer?.disconnect())
</script>

<style scoped>
.products {
  min-height: 100vh;
  padding: 3.5rem max(1.5rem, calc((100% - 1120px) / 2));
  margin: 0 auto;
  background: #fbfaff;
  color: #24145f;
}

.page-heading { display: flex; justify-content: space-between; align-items: end; margin-bottom: 2.3rem; }
.eyebrow { color: #9654db; font-size: .68rem; font-weight: 800; letter-spacing: .18em; }
.page-heading h1 { margin: .55rem 0 .4rem; font-size: clamp(2.8rem, 5vw, 4.7rem); line-height: .9; letter-spacing: -.08em; }
.page-heading h1 span { color: #9654db; }
.page-heading p { margin: 0; color: #8d879e; font-size: .83rem; }
.result-count { color: #8d879e; font-size: .75rem; }
.filters { display: flex; gap: .75rem; flex-wrap: wrap; padding: .85rem; margin-bottom: 2rem; border: 1px solid #e5e0ef; border-radius: 14px; background: #fff; }
.filters select, .filters input { min-height: 42px; padding: 0 .9rem; border: 1px solid #e3dff0; border-radius: 8px; background: #fff; color: #24145f; }
.search-wrap { display: flex; align-items: center; border: 1px solid #e3dff0; border-radius: 8px; background: #fff; }
.search-wrap span { padding-left: .9rem; color: #9654db; font-size: 1.3rem; }
.search-wrap input { border: 0; outline: 0; }
.view-mode-button { margin-left: auto; min-height: 42px; padding: 0 1rem; border: 1px solid #281568; border-radius: 8px; background: #281568; color: #fff; cursor: pointer; font-size: .72rem; font-weight: 700; white-space: nowrap; }
.grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 1.1rem; }
.card { overflow: hidden; border: 1px solid #e9e5f1; border-radius: 14px; background: #fff; box-shadow: 0 8px 24px rgba(36,20,95,.04); transition: transform .25s ease, box-shadow .25s ease; }
.card:hover { transform: translateY(-6px); box-shadow: 0 16px 34px rgba(36,20,95,.12); }
.image-wrap { position: relative; display: block; height: 245px; overflow: hidden; background: #eee9f8; }
.image-wrap img { width: 100%; height: 100%; object-fit: cover; transition: transform .35s ease; }
.card:hover .image-wrap img { transform: scale(1.08); }
.quick-view { position: absolute; right: .8rem; bottom: .8rem; padding: .45rem .65rem; border-radius: 999px; background: #fff; color: #24145f; opacity: 0; font-size: .65rem; transition: opacity .2s ease; }
.card:hover .quick-view { opacity: 1; }
.card-body { position: relative; padding: 1rem; }
.card-topline, .price-row { display: flex; justify-content: space-between; align-items: center; }
.category { margin: 0; color: #9654db; font-size: .65rem; font-weight: 750; text-transform: uppercase; letter-spacing: .08em; }
.card h3 { min-height: 2.3rem; margin: .55rem 0; color: #24145f; font-size: .92rem; line-height: 1.25; }
.price { margin: 0; color: #24145f; font-size: 1.05rem; font-weight: 800; }
.compare-price { margin: 0; color: #aaa5b7; font-size: .72rem; text-decoration: line-through; }
.rating { color: #d89b17; font-size: .7rem; font-weight: 700; }
.discount-badge { position: absolute; top: .8rem; right: .8rem; padding: .35rem .5rem; border-radius: 5px; background: #f24963; color: #fff; font-size: .65rem; font-weight: 700; }
.card button { display: flex; align-items: center; justify-content: center; gap: .5rem; width: 100%; margin-top: .9rem; padding: .65rem; border: 0; border-radius: 8px; background: #281568; color: #fff; cursor: pointer; font-size: .75rem; font-weight: 700; }
.card button span { color: #0fe1d1; font-size: 1.1rem; }
.card button:disabled { background: #cbc7d4; cursor: not-allowed; }
.catalog-footer { display: grid; gap: 1rem; justify-items: center; margin-top: 2rem; padding: 1rem 0 .25rem; }
.pagination { display: flex; justify-content: center; align-items: center; gap: 1rem; color: #796e96; font-size: .75rem; }
.pagination button { padding: .7rem 1rem; border: 0; border-radius: 8px; background: #281568; color: #fff; cursor: pointer; }
.pagination button:disabled { background: #dedbe5; color: #8d879e; cursor: not-allowed; }
.load-more-button { min-width: 210px; padding: .75rem 1.2rem; border: 0; border-radius: 999px; background: #0fe1d1; color: #24145f; font-weight: 800; cursor: pointer; }
.load-more-button:disabled { opacity: .6; cursor: not-allowed; }
.loading-indicator, .scroll-hint, .end-indicator { color: #8d879e; font-size: .75rem; }
.loading-indicator i { display: inline-block; width: .7rem; height: .7rem; margin-right: .35rem; border: 2px solid #d8d1e8; border-top-color: #9654db; border-radius: 50%; animation: spin .7s linear infinite; vertical-align: -1px; }
.catalog-error { display: flex; align-items: center; gap: .75rem; color: #b33450; font-size: .75rem; }
.catalog-error button { padding: .45rem .75rem; border: 1px solid #b33450; border-radius: 999px; background: #fff; color: #b33450; cursor: pointer; }
.initial-error { justify-content: center; margin: 2rem 0; }
@keyframes spin { to { transform: rotate(360deg); } }
@media (max-width: 650px) { .view-mode-button { width: 100%; margin-left: 0; } }
.catalog-layout { display: grid; grid-template-columns: 190px 1fr; gap: 1.5rem; }
.filter-rail { align-self: start; position: sticky; top: 1rem; padding: 1rem; border: 1px solid #e5e0ef; border-radius: 14px; background: #fff; }
.rail-title { display: flex; justify-content: space-between; padding-bottom: 1rem; border-bottom: 1px solid #eeeaf4; color: #24145f; font-size: .65rem; font-weight: 800; letter-spacing: .12em; }
.filter-rail label { display: grid; gap: .5rem; padding-top: 1.2rem; color: #8a8397; font-size: .68rem; font-weight: 700; }
.filter-rail select { width: 100%; padding: .6rem .4rem; border: 0; color: #24145f; font: inherit; font-size: .72rem; outline: 0; }
.rail-note { display: grid; gap: .4rem; margin-top: 3rem; padding: .75rem; border-radius: 9px; background: #f2ecff; color: #24145f; font-size: .66rem; line-height: 1.4; }
.rail-note span { color: #8a8397; }
.loading-state, .load-more-marker { padding: 2rem; color: #8d879e; text-align: center; font-size: .75rem; }
@media (max-width: 850px) { .grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 850px) { .catalog-layout { grid-template-columns: 1fr; } .filter-rail { position: static; } .filter-rail .rail-note { display: none; } }
@media (max-width: 560px) { .page-heading { align-items: start; flex-direction: column; gap: 1rem; } .grid { grid-template-columns: 1fr; } .image-wrap { height: 280px; }
}
</style>
