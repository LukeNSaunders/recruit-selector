<script setup lang="ts">
import type { RecruitResult } from '../types/recruit'

defineProps<{
  result: RecruitResult
}>()
</script>

<template>
  <div class="results">
    <section class="card">
      <h2>Selected recruits</h2>

      <p v-if="result.selectedRecruits.length === 0" class="muted">
        Only one recruit, so no one was selected.
      </p>

      <ol class="selected-list">
        <li v-for="recruit in result.selectedRecruits" :key="recruit">
          {{ recruit }}
        </li>
      </ol>
    </section>

    <section class="card final">
      <h2>Coming back next year</h2>
      <p class="final-number">{{ result.finalRecruit }}</p>
      <p class="muted">Recruit {{ result.finalRecruit }}</p>
    </section>
  </div>
</template>

<style scoped>
.results {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 20px;
}

.results .card {
  margin-bottom: 0;
}

.selected-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.selected-list li {
  min-width: 40px;
  padding: 8px 12px;
  text-align: center;
  background: #eef1ea;
  border-radius: 999px;
  font-weight: 600;
}

.final {
  background: #fdf3dc;
  border-color: #f0d58c;
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.final-number {
  font-size: 64px;
  font-weight: 700;
  line-height: 1;
  margin: 12px 0;
  color: #4b5d3a;
}

@media (max-width: 600px) {
  .results {
    grid-template-columns: 1fr;
  }
}
</style>