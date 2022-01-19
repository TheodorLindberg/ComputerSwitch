import { Button } from '@mui/material'
import Container from '@mui/material/Container'
import Grid from '@mui/material/Grid'
import { Box } from '@mui/system'
import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.css'
import Router from 'next/router'
const Home: NextPage = () => {

  const onClick = () => {
    fetch("/api/on").then(() => console.log("On"))
  }
  const onMonitorClick = () => {
    Router.push("/monitor.html");
  }

  return (
    <Container maxWidth={false} sx={{height:"100vh", background: "#161616"}}>
      <Grid container justifyContent="right" alignItems="bottom" pt={2}>
        <Button onClick={onMonitorClick} size="medium" color="secondary" variant="text">Go to Monitor</Button>
      </Grid>
      <Grid container justifyContent="center" alignItems="center" sx={{height: "calc(100vh - 36.5px - 16px)"}}>
        <Button onClick={onClick} size="large" color="primary" variant="contained"  style={{padding: '24px 66px'}}>Turn on</Button>
      </Grid>
    </Container>
  )
}

export default Home
