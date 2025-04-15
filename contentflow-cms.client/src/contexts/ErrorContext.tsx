import React, { createContext, useContext, useState, ReactNode } from 'react';
import { Alert, Snackbar } from '@mui/material';

interface ErrorContextType {
    showError: (message: string) => void;
}

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);

export const ErrorProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [error, setError] = useState<string | null>(null);
    const [open, setOpen] = useState(false);

    const showError = (message: string) => {
        setError(message);
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
        setError(null);
    };

    return (
        <ErrorContext.Provider value={{ showError }}>
            {children}
            <Snackbar 
                open={open} 
                autoHideDuration={6000} 
                onClose={handleClose}
                anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
            >
                <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
                    {error}
                </Alert>
            </Snackbar>
        </ErrorContext.Provider>
    );
};

export const useError = () => {
    const context = useContext(ErrorContext);
    if (context === undefined) {
        throw new Error('useError must be used within an ErrorProvider');
    }
    return context;
}; 