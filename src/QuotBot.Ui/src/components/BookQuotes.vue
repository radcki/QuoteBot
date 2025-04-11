<script setup lang="ts">
import { ref, onMounted } from "vue";
import BookQuoteService from "@/api/book-quote/BookQuoteService";
import type { BookQuoteDto } from "@/api/book-quote/dto/BookQuoteDto";

const allQuotes = ref<BookQuoteDto[]>([] as BookQuoteDto[]);
const randomQuote = ref<BookQuoteDto>();
const loadDataLoading = ref<boolean>(false);
const randomQuoteLoading = ref<boolean>(false);

const loadData = async () => {
  try {
    loadDataLoading.value = true;
    const data = await BookQuoteService.getAll();
    allQuotes.value = data;
  } catch (e) {
    console.error(e);
  } finally {
    loadDataLoading.value = false;
  }
};
const loadRandomQuote = async () => {
  try {
    randomQuoteLoading.value = true;
    const data = await BookQuoteService.getRandomQuote();
    randomQuote.value = data;
  } catch (e) {
    console.error(e);
  } finally {
    randomQuoteLoading.value = false;
  }
};
onMounted(() => {
  loadRandomQuote();
});
</script>

<template>
  <v-row class="ma-5">
    <v-col :cols="12">
      <div class="d-flex justify-end pa-2">
        <v-btn
          variant="text"
          :loading="randomQuoteLoading"
          icon
          color="primary"
          @click="loadRandomQuote()"
        >
          <v-icon>mdi-reload</v-icon>
        </v-btn>
      </div>
      <book-quote-card
        v-if="randomQuote"
        :quote="randomQuote"
        @on-removed="loadData()"
      />
    </v-col>
    <v-col :cols="12">
      <v-divider />
    </v-col>
    <v-col :cols="12">
      <div class="d-flex justify-center">
        <v-btn
          variant="text"
          :loading="loadDataLoading"
          color="primary"
          @click="loadData()"
        >
          Load all
        </v-btn>
      </div>
    </v-col>

    <template v-for="(quote, quoteIndex) in allQuotes" :key="quoteIndex">
      <v-col :cols="12">
        <book-quote-card :quote="quote" @on-removed="loadData()" />
      </v-col>
    </template>
  </v-row>
</template>
