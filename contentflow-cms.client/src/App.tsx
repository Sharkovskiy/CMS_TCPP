import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ErrorProvider } from './context/ErrorContext';
import ErrorBoundary from './components/ErrorBoundary';
import LoginPage from './components/LoginPage';
import DashboardPage from './components/DashboardPage';
import CategoriesPage from './components/CategoriesPage';
import TagsPage from './components/TagsPage';
import MediaPage from './components/MediaPage';

const App: React.FC = () => {
  return (
    <ErrorProvider>
      <ErrorBoundary>
        <Router>
          <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
            <Route path="/categories" element={<CategoriesPage />} />
            <Route path="/tags" element={<TagsPage />} />
            <Route path="/media" element={<MediaPage />} />
          </Routes>
        </Router>
      </ErrorBoundary>
    </ErrorProvider>
  );
};

export default App;