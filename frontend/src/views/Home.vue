<template>
  <div class="home">
    <section class="hero">
      <div class="hero-copy">
        <p class="eyebrow">THE NEW EVERYDAY STORE</p>
        <h1>Good things,<br /><em>well chosen.</em></h1>
        <p class="hero-description">Thoughtful essentials, fresh finds and everyday upgrades for the way you live now.</p>
        <router-link to="/products" class="primary-button">Explore the collection <span>↗</span></router-link>
        <div class="hero-meta"><span>01 <i></i> 04</span><span>Scroll to discover ↓</span></div>
      </div>

      <div class="hero-art" aria-label="Featured NovaKart collection">
        <div class="art-tile tile-a"></div>
        <div class="art-tile tile-b"><img src="https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=700&q=80" alt="Modern wristwatch" /></div>
        <div class="art-tile tile-c"></div>
        <div class="art-tile tile-d"></div>
        <div class="art-tile tile-e"><img src="https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?auto=format&fit=crop&w=700&q=80" alt="Camera on a desk" /></div>
        <div class="art-circle"></div>
        <div class="art-label">NOVA<br /><strong>EDIT</strong></div>
      </div>

      <div class="hero-bottom"><span>Curated weekly</span><span>Secure checkout</span><span>Made for you</span></div>
    </section>

    <section class="section categories-section">
      <div class="section-heading">
        <div><p class="eyebrow purple">BROWSE BY MOOD</p><h2>Find your next<br /><em>favourite.</em></h2></div>
        <router-link to="/products" class="text-link">View all categories ↗</router-link>
      </div>
      <div class="category-strip" v-if="!catalogStore.loading">
        <router-link v-for="(category, index) in catalogStore.categories.slice(0, 6)" :key="category.id" class="category-card" :class="`category-${index + 1}`" :to="`/products?categoryId=${category.id}`">
          <span class="category-number">0{{ index + 1 }}</span>
          <div><h3>{{ category.name }}</h3><p>{{ category.tagline }}</p></div>
          <span class="category-arrow">↗</span>
        </router-link>
      </div>
      <p v-else class="muted">Loading collections...</p>
    </section>

    <section class="section featured-section">
      <div class="section-heading">
        <div><p class="eyebrow purple">JUST IN</p><h2>Objects with<br /><em>character.</em></h2></div>
        <router-link to="/products" class="text-link">Shop everything ↗</router-link>
      </div>
      <div class="product-grid" v-if="!catalogStore.loading">
        <article v-for="product in featuredProducts.slice(0, 4)" :key="product.id" class="product-card">
          <router-link :to="`/products/${product.id}`" class="product-image">
            <span v-if="product.isNew" class="new-tag">New</span>
            <img :src="productImage(product)" :alt="product.name" />
          </router-link>
          <div class="product-info"><h3>{{ product.name }}</h3><p>{{ product.categoryName || 'NovaKart edit' }}</p><strong>₹{{ product.price.toLocaleString('en-IN') }}</strong></div>
          <button class="add-button" @click="addToCart(product.id)">+</button>
        </article>
      </div>
      <p v-else class="muted">Loading products...</p>
    </section>

    <Newsletter />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import Newsletter from './Newsletter.vue'
import { productImage } from '../utils/productImage'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()
const featuredProducts = computed(() => catalogStore.products.filter(p => p.isFeatured).slice(0, 8))

function addToCart(productId: number) {
  cartStore.addToCart(productId)
}

onMounted(() => {
  catalogStore.fetchCategories()
  catalogStore.fetchProducts({ pageSize: 24 })
})
</script>

<style scoped>
.home { background: #fbfaff; }
.hero { position: relative; overflow: hidden; width: min(1120px, calc(100% - 3rem)); min-height: 570px; margin: 0 auto; padding: 4.8rem 5.5% 2rem; background: #fff; color: #24145f; display: grid; grid-template-columns: 46% 54%; align-items: center; }
.hero-copy { position: relative; z-index: 2; max-width: 440px; }
.eyebrow { margin: 0 0 1.2rem; color: #8b6be0; font-size: .68rem; font-weight: 800; letter-spacing: .18em; }
.hero h1 { margin: 0; font-size: clamp(3.2rem, 5vw, 5.3rem); line-height: .92; letter-spacing: -.08em; font-weight: 850; }
h1 em, h2 em { color: #9654db; font-family: Georgia, serif; font-weight: 400; }
.hero-description { max-width: 320px; margin: 1.8rem 0; color: #766e8f; font-size: .86rem; line-height: 1.65; }
.primary-button { display: inline-flex; align-items: center; gap: 1.2rem; padding: .85rem 1.15rem; border-radius: 999px; background: #281568; color: white; text-decoration: none; font-size: .76rem; font-weight: 700; }
.primary-button span { color: #0fe1d1; font-size: 1.1rem; }
.hero-meta { display: flex; gap: 2rem; margin-top: 5rem; color: #8e88a2; font-size: .63rem; }
.hero-meta span:first-child { color: #24145f; font-weight: 700; }
.hero-meta i { display: inline-block; width: 35px; height: 1px; margin: 0 .35rem; vertical-align: middle; background: #24145f; }
.hero-art { position: relative; min-height: 430px; margin-right: -9%; }
.art-tile { position: absolute; overflow: hidden; }
.tile-a { top: 0; right: 25%; width: 28%; height: 29%; background: #0fe1d1; }
.tile-b { top: 0; right: 0; width: 25%; height: 42%; background: #9858dd; border-radius: 0 0 0 80px; }
.tile-c { top: 29%; left: 10%; width: 35%; height: 36%; background: #281568; border-radius: 100px 0 0 0; }
.tile-d { top: 42%; right: 0; width: 25%; height: 29%; background: #0fe1d1; border-radius: 0 0 0 90px; }
.tile-e { bottom: 0; right: 25%; width: 40%; height: 49%; background: #e8e0fa; border-radius: 110px 0 0 0; }
.art-tile img { width: 100%; height: 100%; object-fit: cover; mix-blend-mode: luminosity; opacity: .88; }
.art-circle { position: absolute; top: 27%; left: 32%; width: 36%; aspect-ratio: 1; border: 25px solid white; border-radius: 50%; background: #b66aeb; box-shadow: inset 0 0 0 1px rgba(36,20,95,.15); }
.art-label { position: absolute; right: 5%; bottom: 10%; color: #fff; font-size: .55rem; line-height: 1.1; letter-spacing: .14em; text-align: center; }
.art-label strong { color: #0fe1d1; }
.hero-bottom { position: absolute; right: 5.5%; bottom: 1.8rem; display: flex; gap: 1.4rem; color: #9a94ac; font-size: .6rem; }
.hero-bottom span::before { content: "✦"; margin-right: .35rem; color: #9654db; }
.section { width: min(1120px, calc(100% - 3rem)); margin: 0 auto; padding: 6rem 5.5% 0; }
.section-heading { display: flex; justify-content: space-between; align-items: end; margin-bottom: 2.4rem; }
.section h2 { margin: 0; color: #24145f; font-size: clamp(2.5rem, 4vw, 4rem); line-height: .9; letter-spacing: -.07em; }
.text-link { color: #24145f; font-size: .74rem; font-weight: 700; text-decoration: none; }
.category-strip { display: grid; grid-template-columns: repeat(3, 1fr); gap: .7rem; }
.category-card { min-height: 150px; padding: 1.3rem; color: #fff; display: flex; flex-direction: column; justify-content: space-between; text-decoration: none; transition: transform .2s ease; }
.category-card:hover { transform: translateY(-5px); }
.category-1, .category-4 { background: #291669; } .category-2, .category-5 { background: #9757dc; } .category-3, .category-6 { background: #0bcec3; color: #24145f; }
.category-number { align-self: end; font-size: .65rem; opacity: .7; }
.category-card h3 { margin: 0; font-size: 1.15rem; letter-spacing: -.04em; }
.category-card p { margin: .35rem 0 0; font-size: .66rem; opacity: .75; }
.category-arrow { display: none; }
.product-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 1rem; }
.product-card { position: relative; }
.product-image { position: relative; display: block; height: 240px; overflow: hidden; background: #ede8f8; }
.product-image img { width: 100%; height: 100%; object-fit: cover; transition: transform .3s ease; }
.product-image:hover img { transform: scale(1.05); }
.image-placeholder { display: grid; width: 100%; height: 100%; place-items: center; color: #9858dd; font: 4rem Georgia, serif; }
.new-tag { position: absolute; z-index: 1; top: .8rem; left: .8rem; padding: .25rem .55rem; background: #0fe1d1; color: #24145f; font-size: .6rem; font-weight: 800; }
.product-info { padding: .85rem 0; }
.product-info h3 { margin: 0 0 .3rem; color: #24145f; font-size: .86rem; }
.product-info p { margin: 0 0 .65rem; color: #9992aa; font-size: .68rem; }
.product-info strong { color: #24145f; font-size: .82rem; }
.add-button { position: absolute; right: 0; bottom: .65rem; width: 28px; height: 28px; border: 0; border-radius: 50%; background: #24145f; color: #fff; cursor: pointer; font-size: 1.1rem; }
.muted { color: #928ca1; }
@media (max-width: 760px) {
  .hero { width: 100%; min-height: 720px; padding: 3.5rem 1.5rem 2rem; display: block; }
  .hero h1 { font-size: 3.65rem; }
  .hero-meta { margin-top: 2rem; }
  .hero-art { min-height: 300px; margin: 3rem -1.5rem 0; }
  .hero-bottom { left: 1.5rem; right: auto; bottom: 1.5rem; }
  .section { width: 100%; padding: 4rem 1.5rem 0; }
  .section-heading { align-items: start; flex-direction: column; gap: 1.25rem; }
  .category-strip, .product-grid { grid-template-columns: repeat(2, 1fr); }
  .product-image { height: 175px; }
}
</style>
