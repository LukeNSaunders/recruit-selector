<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { login } from '../services/auth'

const emit = defineEmits<{
  close: []
  loggedIn: []
}>()

const username = ref('')
const password = ref('')
const error = ref('')

const usernameInput = ref<HTMLInputElement | null>(null)

onMounted(() => {
  usernameInput.value?.focus()
})

async function onSubmit() {
  error.value = ''

  try {
    await login(username.value, password.value)
    emit('loggedIn')
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Something went wrong. Please try again.'
  }
}
</script>

<template>
  <div class="overlay" @click.self="emit('close')">
    <form
      class="card login-box"
      role="dialog"
      aria-modal="true"
      aria-labelledby="login-title"
      @submit.prevent="onSubmit"
    >
      <h2 id="login-title">Log in</h2>

      <label>
        Username
        <input
          ref="usernameInput"
          v-model="username"
          type="text"
          autocomplete="username"
          required
        />
      </label>

      <label>
        Password
        <input v-model="password" type="password" autocomplete="current-password" required />
      </label>

      <p v-if="error" class="error">{{ error }}</p>

      <div class="actions">
        <button type="button" class="secondary" @click="emit('close')">Cancel</button>
        <button type="submit">Log in</button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 16px;
  background: rgba(31, 42, 31, 0.5);
}

.login-box {
  width: 100%;
  max-width: 380px;
  margin-bottom: 0;
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.error {
  color: #a8322a;
  font-weight: 600;
  margin: 0;
}
</style>