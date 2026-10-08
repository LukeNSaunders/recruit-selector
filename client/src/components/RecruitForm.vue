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
form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

label {
  display: flex;
  flex-direction: column;
  gap: 6px;
  font-weight: 600;
  font-size: 14px;
}

input {
  padding: 10px 12px;
  font-size: 16px;
  border: 1px solid #c9cfc5;
  border-radius: 8px;
}

button {
  padding: 12px;
  font-size: 16px;
  font-weight: 600;
  color: #ffffff;
  background: #4b5d3a;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}

button:hover {
  background: #3c4b2e;
}
</style>
