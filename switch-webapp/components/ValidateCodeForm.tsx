import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, TextField } from '@mui/material';
import { request } from 'https';
import React, { ChangeEvent, useEffect, useState } from 'react';
import { useCodeContext } from './CodeProvider';

function ValidateCodeForm() {
  

    const codeContext = useCodeContext();

    const [code, setCode] = useState<string>("")
    const [error, setError] = useState<string | null>(null)

    const [success, setSuccess] = useState<boolean>(false)

  
    const handleClose = () => {
        codeContext.setShowDialog(false)
    }
    const handleValidate = () => {
        fetch(`/api/validate?code=${code}`).then((response) => {
            response.json().then((json) => {
                if(json.valid) {
                    setSuccess(true)
                    codeContext.setCode(code);
                } else {
                    setError("Invalid code")
                }
            })
          
        })
        
    }


    return (<Dialog open={codeContext.showDialog} onClose={handleClose}>
  <DialogTitle>Enter Code</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Please enter your code to access the website
          </DialogContentText>
          <TextField
            autoFocus
            
            error={error != null}
            helperText={error}
            margin="dense"
            id="code"
            label="Code"
            type="text"
            fullWidth
            variant="standard"
            autoComplete='false'
            onChange={(change: ChangeEvent<HTMLInputElement>) => setCode(change.target.value)}
            />
          <Grid container justifyContent="center" mt={2}>
            <Button variant="contained"  sx={{backgroundColor: success ? "green !important" : undefined}} onClick={handleValidate}>Validate</Button>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
        </DialogActions>
    </Dialog>);
}

export default ValidateCodeForm;
