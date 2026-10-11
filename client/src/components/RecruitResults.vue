<script setup lang="ts">
import type { RecruitResult } from '../types/recruit'

defineProps<{
  result: RecruitResult
}>()
</script>

<template>
  <p v-if="result.selectedRecruits.length === 0" class="no-selection">
    Only one recruit was entered, so no one was selected. Enter at least 2 recruits to run a
    selection.
  </p>

  <div v-else class="results">
    <section class="card">
      <h2>Selected recruits</h2>

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
  height: 300px;
  box-sizing: border-box;
  margin-bottom: 0;
  display: flex;
  flex-direction: column;
}

.selected-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-wrap: wrap;
  align-content: flex-start;
  gap: 8px;
  flex: 1;
  min-height: 0;
  overflow-y: auto;
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
  justify-content: center;
}

.final-number {
  font-size: 64px;
  font-weight: 700;
  line-height: 1;
  margin: 12px 0;
  color: #4b5d3a;
}
.no-selection {
  padding: 12px 16px;
  color: #a8322a;
  background: #fbeae8;
  border: 1px solid #f0c4be;
  border-radius: 8px;
  font-weight: 600;
}

@media (max-width: 600px) {
  .results {
    grid-template-columns: 1fr;
  }
}
</style>