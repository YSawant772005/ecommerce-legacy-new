<template>
  <div class="account">
    <p class="eyebrow">YOUR NOVAKART</p>
    <h1>Welcome back,<br><span>{{ authStore.user?.fullName }}</span></h1>
    <div class="account-layout">
      <aside class="account-sidebar">
        <router-link to="/account" class="active">Overview <span>→</span></router-link>
        <router-link to="/account/orders">Orders <span>→</span></router-link>
        <router-link to="/account/profile">Profile <span>→</span></router-link>
        <router-link to="/account/addresses">Addresses <span>→</span></router-link>
        <router-link to="/wishlist">Wishlist <span>→</span></router-link>
      </aside>
      <section>
        <div class="account-info">
          <div class="avatar">{{ authStore.user?.fullName?.charAt(0) }}</div>
          <div><p class="eyebrow">MEMBER PROFILE</p><h2>{{ authStore.user?.fullName }}</h2><p>{{ authStore.user?.email }}</p></div>
        </div>
        <div class="account-cards">
          <router-link to="/account/orders" class="account-card"><small>01</small><strong>Your orders</strong><span>Track and review purchases →</span></router-link>
          <router-link to="/account/addresses" class="account-card"><small>02</small><strong>Saved addresses</strong><span>Make checkout effortless →</span></router-link>
          <router-link to="/account/profile" class="account-card"><small>03</small><strong>Personal details</strong><span>Keep your information current →</span></router-link>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
</script>

<style scoped>
.account {
  min-height: calc(100vh - 100px); padding: 4rem max(1.5rem, calc((100% - 1120px) / 2));
  margin: 0 auto;
  color: #24145f;
  background: #fbfaff;
}

.eyebrow { color: #9654db; font-size: .68rem; font-weight: 800; letter-spacing: .18em; }
.account h1 { margin: .5rem 0 3rem; font-size: clamp(2.8rem, 6vw, 5.5rem); line-height: .9; letter-spacing: -.08em; }
.account h1 span { color: #9654db; }
.account-layout { display:grid; grid-template-columns: 220px 1fr; gap: 3rem; }
.account-sidebar { display:grid; align-content:start; gap:.3rem; }
.account-sidebar a { display:flex; justify-content:space-between; color:#6d6389; text-decoration:none; padding: .9rem 0; border-bottom:1px solid #e8e3f1; font-weight:700; }
.account-sidebar a.active, .account-sidebar a:hover { color:#24145f; }
.account-info {
  display:flex; align-items:center; gap:1.2rem; background:#fff; padding:2rem;
  border: 1px solid #e8e3f1;
  border-radius: 16px;
  margin-bottom: 2rem;
  box-shadow: 0 12px 30px rgba(36,20,95,.06);
}
.avatar { display:grid; place-items:center; width:62px; height:62px; border-radius:50%; background:#0fe1d1; color:#24145f; font-size:1.7rem; font-weight:800; }
.account-info h2 { margin:.3rem 0; }
.account-info p:last-child { margin:0; color:#796e96; }
.account-cards { display:grid; grid-template-columns:repeat(3,1fr); gap:1rem; margin-top:1rem; }
.account-card { display:grid; gap:.7rem; min-height:150px; padding:1.4rem; color:#24145f; background:#fff; border:1px solid #e8e3f1; border-radius:16px; text-decoration:none; transition:transform .2s, box-shadow .2s; }
.account-card:hover { transform:translateY(-4px); box-shadow:0 14px 30px rgba(36,20,95,.1); }
.account-card small { color:#9654db; font-weight:800; }
.account-card span { color:#796e96; font-size:.8rem; }

.account-info p {
  margin: 0.5rem 0;
}

@media (max-width:760px) {
  .account-layout, .account-cards { grid-template-columns:1fr; }
}
</style>
