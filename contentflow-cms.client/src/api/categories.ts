import axios from 'axios';
import { useError } from '../contexts/ErrorContext';

const API_URL = 'http://localhost:5000/api';

export interface Category {
  id: number;
  name: string;
  description?: string;
}

const handleError = (error: any, showError: (message: string) => void) => {
  if (axios.isAxiosError(error)) {
    if (error.response) {
      // The request was made and the server responded with a status code
      // that falls out of the range of 2xx
      showError(error.response.data?.message || 'Server error occurred');
    } else if (error.request) {
      // The request was made but no response was received
      showError('No response from server. Please check your connection.');
    } else {
      // Something happened in setting up the request that triggered an Error
      showError('Error setting up the request');
    }
  } else {
    showError('An unexpected error occurred');
  }
};

export const getCategories = async (showError: (message: string) => void): Promise<Category[]> => {
  try {
    const response = await axios.get(`${API_URL}/categories`);
    return response.data;
  } catch (error) {
    handleError(error, showError);
    return [];
  }
};

export const getCategory = async (id: number, showError: (message: string) => void): Promise<Category | null> => {
  try {
    const response = await axios.get(`${API_URL}/categories/${id}`);
    return response.data;
  } catch (error) {
    handleError(error, showError);
    return null;
  }
};

export const createCategory = async (category: Omit<Category, 'id'>, showError: (message: string) => void): Promise<Category | null> => {
  try {
    const response = await axios.post(`${API_URL}/categories`, category);
    return response.data;
  } catch (error) {
    handleError(error, showError);
    return null;
  }
};

export const updateCategory = async (id: number, category: Category, showError: (message: string) => void): Promise<boolean> => {
  try {
    await axios.put(`${API_URL}/categories/${id}`, category);
    return true;
  } catch (error) {
    handleError(error, showError);
    return false;
  }
};

export const deleteCategory = async (id: number, showError: (message: string) => void): Promise<boolean> => {
  try {
    await axios.delete(`${API_URL}/categories/${id}`);
    return true;
  } catch (error) {
    handleError(error, showError);
    return false;
  }
}; 