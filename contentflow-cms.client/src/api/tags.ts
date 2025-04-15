import axios from 'axios';

const API_URL = 'http://localhost:5000/api';

export interface Tag {
  id: number;
  name: string;
}

export const getTags = async (): Promise<Tag[]> => {
  const response = await axios.get(`${API_URL}/tags`);
  return response.data;
};

export const getTag = async (id: number): Promise<Tag> => {
  const response = await axios.get(`${API_URL}/tags/${id}`);
  return response.data;
};

export const createTag = async (tag: Omit<Tag, 'id'>): Promise<Tag> => {
  const response = await axios.post(`${API_URL}/tags`, tag);
  return response.data;
};

export const updateTag = async (id: number, tag: Tag): Promise<void> => {
  await axios.put(`${API_URL}/tags/${id}`, tag);
};

export const deleteTag = async (id: number): Promise<void> => {
  await axios.delete(`${API_URL}/tags/${id}`);
}; 