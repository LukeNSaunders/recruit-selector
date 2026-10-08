<script setup lang="ts">
import { ref } from 'vue'
import RecruitForm from './components/RecruitForm.vue'
import RecruitResults from './components/RecruitResults.vue'
import { selectRecruits } from './services/recruitApi'
import type { RecruitResult } from './types/recruit'

const result = ref<RecruitResult | null>(null)
const error = ref('')

async function runSelection(recruits: number, skip: number) {
  error.value = ''
  result.value = null

  try {
    result.value = await selectRecruits(recruits, skip)
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Something went wrong. Please try again.'
  }
}
</script>

<template>
  <main class="page">
    <h1>Who Comes Back Next Year?</h1>
    <p class="intro">
      Enter how many recruits there are and how many the sergeant skips each time. We'll show who
      gets a position, in order, and the one recruit who has to come back next year.
    </p>

    <RecruitForm @submit="runSelection" />

    <p v-if="error" class="error">{{ error }}</p>

    <RecruitResults v-if="result" :result="result" />
  </main>
</template>

<style scoped>
.page {
  max-width: 800px;
  margin: 48px auto;
  padding: 0 16px;
}

.intro {
  color: #5a645a;
  line-height: 1.5;
  margin-bottom: 24px;
}

.error {
  color: #a8322a;
  font-weight: 600;
}
</style>