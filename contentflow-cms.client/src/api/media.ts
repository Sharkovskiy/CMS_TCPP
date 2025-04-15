import axios from 'axios';
import { useError } from '../context/ErrorContext';

const API_URL = 'http://localhost:5000/api';

export interface Media {
  id: number;
  fileName: string;
  fileType: string;
  filePath: string;
  fileSize: number;
  uploadDate: string;
}

export const getMedia = async (): Promise<Media[]> => {
  try {
    const response = await axios.get(`${API_URL}/media`);
    return response.data;
  } catch (error) {
    return [];
  }
};

export const getMediaItem = async (id: number): Promise<Media | null> => {
  try {
    const response = await axios.get(`${API_URL}/media/${id}`);
    return response.data;
  } catch (error) {
    return null;
  }
};

export const uploadMedia = async (file: File): Promise<Media | null> => {
  try {
    const formData = new FormData();
    formData.append('file', file);
    
    const response = await axios.post(`${API_URL}/media`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error) {
    return null;
  }
};

export const deleteMedia = async (id: number): Promise<boolean> => {
  try {
    await axios.delete(`${API_URL}/media/${id}`);
    return true;
  } catch (error) {
    return false;
  }
}; 