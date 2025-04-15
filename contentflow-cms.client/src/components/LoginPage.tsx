import React, { useState } from 'react';
import { Box, TextField, Checkbox, FormControlLabel, Button, Typography, Link, Divider } from '@mui/material';
import { styled } from '@mui/material/styles';
import GoogleIcon from '@mui/icons-material/Google';
import AppleIcon from '@mui/icons-material/Apple';
import TwitterIcon from '@mui/icons-material/Twitter';
import { useNavigate } from 'react-router-dom';

const LoginContainer = styled(Box)(({ theme }) => ({
    maxWidth: 400,
    margin: 'auto',
    padding: theme.spacing(3),
    borderRadius: theme.shape.borderRadius,
    boxShadow: theme.shadows[3],
    backgroundColor: '#fff',
    textAlign: 'center',
    marginTop: theme.spacing(10),
}));

const SignInButton = styled(Button)(({ theme }) => ({
    backgroundColor: '#1A3C34',
    color: '#fff',
    padding: theme.spacing(1.5),
    marginTop: theme.spacing(2),
    '&:hover': {
        backgroundColor: '#2A5C54',
    },
}));

const SocialButton = styled(Button)(({ theme }) => ({
    borderColor: theme.palette.grey[400],
    color: theme.palette.grey[800],
    margin: theme.spacing(1),
    padding: theme.spacing(1),
}));

const LoginPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const navigete = useNavigate();

    const handleSignIn = () => {
        console.log('Sign in with:', { email, password, rememberMe });
        navigete("/dashboard")
    };

    return (
        <LoginContainer>
            <Typography variant="h5" gutterBottom>
                ContentFlow CMS
            </Typography>
            <Typography variant="h6" gutterBottom>
                Welcome back
            </Typography>
            <TextField
                label="Email"
                variant="outlined"
                fullWidth
                margin="normal"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="name@company.com"
            />
            <TextField
                label="Password"
                type="password"
                variant="outlined"
                fullWidth
                margin="normal"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <Box display="flex" justifyContent="space-between" alignItems="center" mt={1}>
                <FormControlLabel
                    control={<Checkbox checked={rememberMe} onChange={(e) => setRememberMe(e.target.checked)} />}
                    label="Remember me"
                />
                <Link href="#" variant="body2" color="primary">
                    Forgot password?
                </Link>
            </Box>
            <SignInButton variant="contained" fullWidth onClick={handleSignIn}>
                Sign in
            </SignInButton>
            <Typography variant="body2" mt={2}>
                Don’t have an account? <Link href="#" color="primary">Sign up</Link>
            </Typography>
            <Divider sx={{ my: 2 }}>
                <Typography variant="body2" color="textSecondary">
                    Or continue with
                </Typography>
            </Divider>
            <Box display="flex" justifyContent="center">
                <SocialButton variant="outlined">
                    <GoogleIcon />
                </SocialButton>
                <SocialButton variant="outlined">
                    <AppleIcon />
                </SocialButton>
                <SocialButton variant="outlined">
                    <TwitterIcon />
                </SocialButton>
            </Box>
        </LoginContainer>
    );
};

export default LoginPage;