<script setup lang="ts">
import { ref } from "vue";
import LoginModal from "./components/LoginModal.vue";
import RecruitForm from "./components/RecruitForm.vue";
import RecruitResults from "./components/RecruitResults.vue";
import { getToken, logout } from "./services/auth";
import { selectRecruits } from "./services/recruitApi";
import type { RecruitResult } from "./types/recruit";

const isLoggedIn = ref(getToken() !== null);
const showLogin = ref(false);
const result = ref<RecruitResult | null>(null);
const error = ref("");
const success = ref("");

function handleLoggedIn() {
  isLoggedIn.value = true;
  showLogin.value = false;
  error.value = "";
  success.value = "Successfully logged in!";

  setTimeout(() => {
    success.value = "";
  }, 3000);
}

function handleLogout() {
  logout();
  isLoggedIn.value = false;
  result.value = null;
  error.value = "";
  success.value = "Successfully logged out!";

  setTimeout(() => {
    success.value = "";
  }, 3000);
}

async function runSelection(recruits: number, skip: number) {
  error.value = "";
  success.value = "";
  result.value = null;

  try {
    result.value = await selectRecruits(recruits, skip);
  } catch (e) {
    error.value =
      e instanceof Error ? e.message : "Something went wrong. Please try again.";

    isLoggedIn.value = getToken() !== null;
    if (!isLoggedIn.value) {
      showLogin.value = true;
    }
  }
}
</script>

<template>
  <main class="page">
    <header class="header">
      <h1>Who Comes Back Next Year?</h1>
      <button v-if="isLoggedIn" class="secondary" @click="handleLogout">Log out</button>
      <button v-else class="secondary" @click="showLogin = true">Log in</button>
    </header>

    <p class="intro">
      Enter how many recruits there are and how many the sergeant skips each time. We'll show who
      gets a position, in order, and the one recruit who has to come back next year.
    </p>

    <RecruitForm @submit="runSelection" />

    <p v-if="success" class="success">{{ success }}</p>
    <p v-if="error" class="error">{{ error }}</p>

    <!-- Fills the rest of the screen -->
    <div v-if="result" class="results-area">
      <RecruitResults :result="result" />
    </div>

    <LoginModal v-if="showLogin" @close="showLogin = false" @logged-in="handleLoggedIn" />
  </main>
</template>

<style scoped>
.page {
  max-width: 1000px;
  height: 100vh;
  box-sizing: border-box;
  margin: 0 auto;
  padding: 24px 16px;
  display: flex;
  flex-direction: column;
}

.results-area {
  flex: 1;
  min-height: 0;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
}

.header button {
  padding: 8px 16px;
  font-size: 14px;
}

h1 {
  margin: 0;
}

.intro {
  color: #5a645a;
  line-height: 1.5;
  margin: 8px 0 16px;
}

.success {
  margin: 0 0 16px;
  padding: 12px 16px;
  color: #2f4a22;
  background: #e6f0de;
  border: 1px solid #b9d3a8;
  border-radius: 8px;
  font-weight: 600;
}

.error {
  margin: 0 0 16px;
  color: #a8322a;
  font-weight: 600;
}

@media (max-width: 700px) {
  .page {
    height: auto;
  }
}
</style>