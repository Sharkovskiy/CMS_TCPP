import React, { useState, useEffect } from 'react';
import { useError } from '../context/ErrorContext';
import { getMedia, uploadMedia, deleteMedia, Media } from '../api/media';

const MediaPage: React.FC = () => {
  const [media, setMedia] = useState<Media[]>([]);
  const [loading, setLoading] = useState(true);
  const { showError } = useError();

  useEffect(() => {
    fetchMedia();
  }, []);

  const fetchMedia = async () => {
    try {
      setLoading(true);
      const mediaData = await getMedia();
      setMedia(mediaData);
    } catch (error) {
      showError('Failed to fetch media items');
    } finally {
      setLoading(false);
    }
  };

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    try {
      const uploadedMedia = await uploadMedia(file);
      if (uploadedMedia) {
        setMedia([...media, uploadedMedia]);
      }
    } catch (error) {
      showError('Failed to upload media');
    }
  };

  const handleDelete = async (id: number) => {
    try {
      const success = await deleteMedia(id);
      if (success) {
        setMedia(media.filter(item => item.id !== id));
      }
    } catch (error) {
      showError('Failed to delete media item');
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>Media Library</h1>
      <input
        type="file"
        onChange={handleFileUpload}
        accept="image/*,video/*"
      />
      <div className="media-grid">
        {media.map(item => (
          <div key={item.id} className="media-item">
            {item.fileType.startsWith('image/') ? (
              <img src={item.filePath} alt={item.fileName} />
            ) : (
              <video src={item.filePath} controls />
            )}
            <p>{item.fileName}</p>
            <button onClick={() => handleDelete(item.id)}>Delete</button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default MediaPage; 