<script setup lang="ts">
import BookQuoteService from "@/api/book-quote/BookQuoteService";
import type { BookQuoteDto } from "@/api/book-quote/dto/BookQuoteDto";
import { defineProps, computed, defineEmits } from "vue";

const props = defineProps<{ quote: BookQuoteDto }>();
const quote = computed(() => props.quote);
const emit = defineEmits(["onRemoved"]);
const removeQuoteLoading = ref<boolean>(false);

const removeQuote = async (quote: BookQuoteDto) => {
  try {
    removeQuoteLoading.value = true;
    await BookQuoteService.delete(quote);
    emit("onRemoved");
  } catch (e) {
    console.error(e);
  } finally {
    removeQuoteLoading.value = false;
  }
};
const sendAsNotification = async (quote: BookQuoteDto) => {
  try {
    await BookQuoteService.sendAsNotification(quote);
  } catch (e) {
    console.error(e);
  }
};
</script>

<template>
  <v-card>
    <v-list-item>
      <v-list-item-title class="px-3 pt-3">
        {{ quote.author }} - {{ quote.bookTitle }}
      </v-list-item-title>
      <v-list-item-subtitle class="pa-3">
        {{ quote.content }}
      </v-list-item-subtitle>
      <template #append="{}">
        <v-list-item-action class="flex-column align-end">
          <v-btn
            variant="text"
            :loading="removeQuoteLoading"
            icon
            color="error"
            @click="removeQuote(quote)"
          >
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </v-list-item-action>
        <v-list-item-action class="flex-column align-end">
          <v-btn
            variant="text"
            icon
            color="primary"
            @click="sendAsNotification(quote)"
          >
            <v-icon>mdi-share</v-icon>
          </v-btn>
        </v-list-item-action>
      </template>
    </v-list-item>
  </v-card>
</template>
