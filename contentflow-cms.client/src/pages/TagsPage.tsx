import React, { useState, useEffect } from 'react';
import { useError } from '../context/ErrorContext';
import { getTags, createTag, updateTag, deleteTag, Tag } from '../api/tags';

const TagsPage: React.FC = () => {
    const [tags, setTags] = useState<Tag[]>([]);
    const [loading, setLoading] = useState(true);
    const { showError } = useError();

    useEffect(() => {
        fetchTags();
    }, []);

    const fetchTags = async () => {
        try {
            setLoading(true);
            const tagsData = await getTags();
            setTags(tagsData);
        } catch (error) {
            showError('Failed to fetch tags');
        } finally {
            setLoading(false);
        }
    };

    const handleCreate = async (name: string) => {
        try {
            const newTag = await createTag({ name });
            setTags([...tags, newTag]);
        } catch (error) {
            showError('Failed to create tag');
        }
    };

    const handleUpdate = async (id: number, name: string) => {
        try {
            await updateTag(id, { id, name });
            setTags(tags.map(tag => tag.id === id ? { ...tag, name } : tag));
        } catch (error) {
            showError('Failed to update tag');
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await deleteTag(id);
            setTags(tags.filter(tag => tag.id !== id));
        } catch (error) {
            showError('Failed to delete tag');
        }
    };

    return (
        <div>
            <h1>Tags</h1>
            {loading ? (
                <div>Loading...</div>
            ) : (
                <div>
                    {/* Add your tags UI here */}
                    {tags.map(tag => (
                        <div key={tag.id}>
                            {tag.name}
                            <button onClick={() => handleDelete(tag.id)}>Delete</button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default TagsPage; 