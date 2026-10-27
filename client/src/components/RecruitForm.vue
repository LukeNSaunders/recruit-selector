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
  <form class="card recruit-form" @submit.prevent="onSubmit">
    <label>
      <span>Number of recruits <span class="hint">1 to 100</span></span>
      <input v-model.number="recruits" type="number" min="1" max="100" required />
    </label>

    <label>
      <span>Recruits to skip <span class="hint">1 to 100</span></span>
      <input v-model.number="skip" type="number" min="1" max="100" required />
    </label>

    <button type="submit">Run selection</button>
  </form>
</template>

<style scoped>
.recruit-form {
  margin-bottom: 0;
}

.hint {
  margin-left: 6px;
  font-size: 13px;
  font-weight: 400;
  color: #7a8475;
}
</style>