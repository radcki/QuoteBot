import api from "@/api/ApiService";
import type { BookQuoteDto } from "./dto/BookQuoteDto";

class BookQuoteService {
  private baseUrl = "quote/";

  async getAll(): Promise<BookQuoteDto[]> {
    const url = this.baseUrl + "";
    return (await api.get<BookQuoteDto[]>(url, null)).data;
  }
  async getRandomQuote(): Promise<BookQuoteDto> {
    const url = this.baseUrl + "random";
    return (await api.get<BookQuoteDto>(url, null)).data;
  }
  async delete(quote: BookQuoteDto): Promise<void> {
    const url = this.baseUrl + quote.bookQuoteId;
    return (await api.delete<void>(url)).data;
  }
  async sendAsNotification(quote: BookQuoteDto): Promise<void> {
    const url = this.baseUrl + quote.bookQuoteId + "/send-as-notification/";
    return (await api.post<void>(url, {})).data;
  }
}

export default new BookQuoteService();
