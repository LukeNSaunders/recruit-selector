<script setup lang="ts">
import { ref } from "vue";

const emit = defineEmits<{
  submit: [n: number, k: number];
}>();

const recruits = ref(7);
const skip = ref(2);

function onSubmit() {
  emit("submit", recruits.value, skip.value);
}
</script>

<template>
  <form class="card" @submit.prevent="onSubmit">
    <label>
      Number of recruits (n)
      <input v-model.number="recruits" type="number" min="1" max="100" required />
      <span class="hint">Between 1 and 100</span>
    </label>

    <label>
      Recruits to skip (k)
      <input v-model.number="skip" type="number" min="0" max="100" required />
      <span class="hint">Between 0 and 100</span>
    </label>

    <button type="submit">Run selection</button>
  </form>
</template>

<style scoped>
.hint {
  font-size: 13px;
  font-weight: 400;
  color: #7a8475;
}
</style>