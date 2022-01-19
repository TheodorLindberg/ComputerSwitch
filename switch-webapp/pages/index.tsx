import { Button } from '@mui/material'
import Container from '@mui/material/Container'
import Grid from '@mui/material/Grid'
import { Box } from '@mui/system'
import type { NextPage } from 'next'
import Head from 'next/head'
import Image from 'next/image'
import styles from '../styles/Home.module.css'

const Home: NextPage = () => {

  const onClick = () => {
    fetch("/api/on").then(() => console.log("On"))
  }

  return (
    <Container maxWidth={false} sx={{height:"100vh", background: "#161616"}}>
      <Grid container justifyContent="center" alignItems="center" sx={{height: "100vh"}}>
        <Button onClick={onClick} size="large" color="primary" variant="contained"  style={{padding: '24px 66px'}}>Turn on</Button>
      </Grid>
    </Container>
  )
}

export default Home
