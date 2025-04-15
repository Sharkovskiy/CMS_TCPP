import React, { useEffect, useState } from 'react';
import { getCategories, createCategory, updateCategory, deleteCategory, Category } from '../api/categories';
import { useError } from '../contexts/ErrorContext';

const CategoriesPage: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [newCategory, setNewCategory] = useState({ name: '', description: '' });
  const [editingCategory, setEditingCategory] = useState<Category | null>(null);
  const { showError } = useError();

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    const data = await getCategories(showError);
    if (data) {
      setCategories(data);
    }
  };

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await createCategory(newCategory, showError);
    if (result) {
      setNewCategory({ name: '', description: '' });
      loadCategories();
    }
  };

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingCategory) {
      const success = await updateCategory(editingCategory.id, editingCategory, showError);
      if (success) {
        setEditingCategory(null);
        loadCategories();
      }
    }
  };

  const handleDelete = async (id: number) => {
    const success = await deleteCategory(id, showError);
    if (success) {
      loadCategories();
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Categories</h1>
      
      {/* Create Category Form */}
      <form onSubmit={handleCreate} className="mb-4">
        <input
          type="text"
          placeholder="Category Name"
          value={newCategory.name}
          onChange={(e) => setNewCategory({ ...newCategory, name: e.target.value })}
          className="border p-2 mr-2"
          required
        />
        <input
          type="text"
          placeholder="Description"
          value={newCategory.description}
          onChange={(e) => setNewCategory({ ...newCategory, description: e.target.value })}
          className="border p-2 mr-2"
        />
        <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
          Add Category
        </button>
      </form>

      {/* Categories List */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {categories.map((category) => (
          <div key={category.id} className="border p-4 rounded">
            {editingCategory?.id === category.id ? (
              <form onSubmit={handleUpdate}>
                <input
                  type="text"
                  value={editingCategory.name}
                  onChange={(e) => setEditingCategory({ ...editingCategory, name: e.target.value })}
                  className="border p-2 mb-2 w-full"
                  required
                />
                <input
                  type="text"
                  value={editingCategory.description || ''}
                  onChange={(e) => setEditingCategory({ ...editingCategory, description: e.target.value })}
                  className="border p-2 mb-2 w-full"
                />
                <div className="flex gap-2">
                  <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded">
                    Save
                  </button>
                  <button
                    type="button"
                    onClick={() => setEditingCategory(null)}
                    className="bg-gray-500 text-white px-4 py-2 rounded"
                  >
                    Cancel
                  </button>
                </div>
              </form>
            ) : (
              <>
                <h3 className="text-xl font-semibold">{category.name}</h3>
                {category.description && <p className="text-gray-600">{category.description}</p>}
                <div className="mt-2">
                  <button
                    onClick={() => setEditingCategory(category)}
                    className="bg-yellow-500 text-white px-3 py-1 rounded mr-2"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(category.id)}
                    className="bg-red-500 text-white px-3 py-1 rounded"
                  >
                    Delete
                  </button>
                </div>
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default CategoriesPage; 