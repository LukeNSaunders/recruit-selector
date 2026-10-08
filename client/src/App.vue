<script setup lang="ts">
import { ref } from 'vue'

const message = ref('')

async function checkServer() {
  const response = await fetch('/api/health')
  message.value = await response.text()
}
</script>

<template>
  <h1>Recruit Selector</h1>
  <button @click="checkServer">Check server</button>
  <p>{{ message }}</p>
</template>
