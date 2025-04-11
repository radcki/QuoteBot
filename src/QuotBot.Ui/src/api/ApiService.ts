import axios from "axios";

class ApiService {
  private baseUrl = `${import.meta.env.VITE_APIURL}/`;
  private userManager: any = null;

  async get<T = any>(url: string, query?: any) {
    if (query) {
      const params: string[] = [];
      for (const key in query) {
        if (Array.isArray(query[key])) {
          const valueList = query[key].join(",");
          params.push(`${key}=${this.ToString(valueList)}`);
        } else if (query[key]) {
          params.push(`${key}=${this.ToString(query[key])}`);
        }
      }
      if (params.length > 0) {
        url += "?" + params.join("&");
      }
    }
    return await axios.get<T>(this.baseUrl + url);
  }

  async post<T = any>(url: string, data: any) {
    return await axios.post<T>(this.baseUrl + url, data || {});
  }
  async delete<T = any>(url: string) {
    return await axios.delete<T>(this.baseUrl + url);
  }

  async patch<T = any>(url: string, data: any) {
    return await axios.patch<T>(this.baseUrl + url, data || {});
  }

  private ToString(value: any) {
    if (value instanceof Date) {
      return value.toJSON();
    }
    return value.toString();
  }
}

const api = new ApiService();

export default api;
