import { Button } from '@mui/material'
import Container from '@mui/material/Container'
import Grid from '@mui/material/Grid'
import { Box } from '@mui/system'
import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.css'
import Router from 'next/router'
import ValidateCodeForm from '../components/ValidateCodeForm'
import { useCodeContext } from '../components/CodeProvider'

const Home: NextPage = () => {
  const codeContext = useCodeContext();
  
  const onClick = () => {
    fetch(`/api/on?code=${codeContext.code}`).then((response) =>{
      if(response.status == 200)
        alert("Turing on computer");
      else
        codeContext.setShowDialog(true)
    })
  }
  const onMonitorClick = () => {
    Router.push("/monitor.html");
  }

  return (
    <Container maxWidth={false} sx={{height:"100vh", background: "#161616"}}>
      <Grid container justifyContent="right" alignItems="bottom" pt={2}>
        <Button onClick={onMonitorClick} size="medium" color="primary" variant="text" sx={{mr: 2}}>Go to Monitor</Button>
        <Button onClick={() => codeContext.setShowDialog(true)} size="medium" color="secondary" variant="text">Code</Button>
      </Grid>
      <Grid container justifyContent="center" alignItems="center" sx={{height: "calc(100vh - 36.5px - 16px)"}}>
        <Button onClick={onClick} size="large" color="primary" variant="contained"  style={{padding: '24px 66px'}}>Turn on</Button>
      </Grid>
      <ValidateCodeForm/>
    </Container>
  )
}

export default Home
