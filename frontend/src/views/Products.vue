<template>
  <div class="products">
    <div class="page-heading">
      <div><span class="eyebrow">THE NOVAKART EDIT</span><h1>Shop the collection<span>.</span></h1><p>Everyday objects, selected with intention.</p></div>
      <span class="result-count">{{ catalogStore.totalCount }} products</span>
    </div>

    <div class="filters">
      <select v-model="selectedCategory" @change="loadProducts">
        <option :value="null">All Categories</option>
        <option v-for="category in catalogStore.categories" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>

      <div class="search-wrap"><span>⌕</span><input v-model="searchQuery" placeholder="Search products..." @input="loadProducts" /></div>

      <select v-model="sortBy" @change="loadProducts">
        <option value="popular">Popular</option>
        <option value="price-asc">Price: Low to High</option>
        <option value="price-desc">Price: High to Low</option>
        <option value="rating">Rating</option>
        <option value="new">Newest</option>
      </select>
    </div>

    <div class="grid" v-if="!catalogStore.loading">
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
          <button @click="addToCart(product.id)" :disabled="!product.inStock"><span>+</span>{{ product.inStock ? 'Add to cart' : 'Out of stock' }}</button>
        </div>
      </div>
    </div>
    <p v-else>Loading...</p>

    <div class="pagination" v-if="catalogStore.totalPages > 1">
      <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
      <span>Page {{ currentPage }} of {{ catalogStore.totalPages }}</span>
      <button @click="nextPage" :disabled="currentPage === catalogStore.totalPages">Next</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import { productImage } from '../utils/productImage'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()

const selectedCategory = ref<number | null>(null)
const searchQuery = ref('')
const sortBy = ref('popular')
const currentPage = ref(1)

function loadProducts() {
  catalogStore.fetchProducts({
    page: currentPage.value,
    pageSize: 24,
    categoryId: selectedCategory.value || undefined,
    search: searchQuery.value || undefined,
    sort: sortBy.value
  })
}

function addToCart(productId: number) {
  cartStore.addToCart(productId)
}

function prevPage() {
  if (currentPage.value > 1) {
    currentPage.value--
    loadProducts()
  }
}

function nextPage() {
  if (currentPage.value < catalogStore.totalPages) {
    currentPage.value++
    loadProducts()
  }
}

onMounted(() => {
  catalogStore.fetchCategories()
  loadProducts()
})
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
.pagination button { border: 0; border-radius: 8px; background: #281568; color: #fff; }
.pagination button:disabled { background: #dedbe5; }
@media (max-width: 850px) { .grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .page-heading { align-items: start; flex-direction: column; gap: 1rem; } .grid { grid-template-columns: 1fr; } .image-wrap { height: 280px; }
}
</style>
