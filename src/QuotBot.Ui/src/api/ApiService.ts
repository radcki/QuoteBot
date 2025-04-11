import axios from "axios";

class ApiService {
  private baseUrl = `${import.meta.env.VITE_APIURL || "/api"}/`;
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

  async postMultipart<T = any>(resource: string, data: any) {
    const formData = new FormData();
    Object.keys(data)
      .filter((property) => !!data[property])
      .forEach((property) => {
        const propertyValue = Reflect.get(data, property);
        if (Array.isArray(propertyValue)) {
          formData.append(property, this.propertyToString(propertyValue));
          let i = 0;
          for (const item of propertyValue) {
            formData.append(`${property}[${i}]`, this.propertyToString(item));
            i++;
          }
        } else if (propertyValue instanceof File) {
          formData.append(property, propertyValue);
        } else {
          formData.append(property, this.propertyToString(propertyValue));
        }
      });
    return axios.post<T>(resource, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      baseURL: this.baseUrl,
    });
  }

  private propertyToString(property: any): string {
    if (property instanceof Date) return property.toJSON();
    if (property.toString) return property.toString();
    return property;
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
