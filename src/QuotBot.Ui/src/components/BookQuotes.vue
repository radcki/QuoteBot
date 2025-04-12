<script setup lang="ts">
import { ref, onMounted } from "vue";
import BookQuoteService from "@/api/book-quote/BookQuoteService";
import type { BookQuoteDto } from "@/api/book-quote/dto/BookQuoteDto";

const allQuotes = ref<BookQuoteDto[]>([] as BookQuoteDto[]);
const randomQuote = ref<BookQuoteDto>();
const loadDataLoading = ref<boolean>(false);
const randomQuoteLoading = ref<boolean>(false);
const deleteAllQuotesLoading = ref<boolean>(false);
const uploadKindleClippingsLoading = ref<boolean>(false);
const clippingsInputFile = ref<File>();

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
const deleteAllQuotes = async () => {
  try {
    deleteAllQuotesLoading.value = true;
    await BookQuoteService.deleteAll();
    loadData();
  } catch (e) {
    console.error(e);
  } finally {
    deleteAllQuotesLoading.value = false;
  }
};
const uploadKindleClippings = async () => {
  try {
    uploadKindleClippingsLoading.value = true;
    if (clippingsInputFile.value) {
      const reader = new FileReader();
      reader.onload = () => {
        const clippingsText = reader.result as string;
        BookQuoteService.uploadKindleClippingsAsString(clippingsText).then(
          () => {
            loadData();
          }
        );
      };
      reader.readAsText(clippingsInputFile.value);
    }
  } catch (e) {
    console.error(e);
  } finally {
    uploadKindleClippingsLoading.value = false;
  }
};
onMounted(() => {
  loadRandomQuote();
});
</script>

<template>
  <v-row class="ma-5">
    <v-col :cols="12">
      <div class="d-flex pa-2">
        <div class="d-flex flex-grow-0">
          <v-file-input
            v-model="clippingsInputFile"
            accept=".txt"
            width="300"
            label="Upload kindle clippings"
          />
        </div>
        <div class="pa-2">
          <v-btn
            variant="text"
            :disabled="!clippingsInputFile"
            :loading="uploadKindleClippingsLoading"
            icon
            color="primary"
            @click="uploadKindleClippings()"
          >
            <v-icon>mdi-upload</v-icon>
          </v-btn>
        </div>
        <div class="pa-2">
          <v-btn
            variant="text"
            :loading="deleteAllQuotesLoading"
            icon
            color="error"
            @click="deleteAllQuotes()"
          >
            <v-icon>mdi-delete-alert</v-icon>
          </v-btn>
        </div>
      </div>
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
