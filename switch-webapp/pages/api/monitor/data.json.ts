// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'

type Data = {
  name: string
}


export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Data>
) {

    const data = await fetch("http://192.168.10.228:3001/data.json")
    const json = await data.json()
    res.status(data.status).json(json)
}
