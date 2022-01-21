// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'

type Data = {
  name: string
}


export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Data>
) {

  console.log(req.cookies)
    if(req.query.code != process.env.code && req.cookies.code != process.env.code)
      return res.status(403).send({message: "invalidCode"} as any);

    const data = await fetch("http://192.168.10.228:3001/data.json")
    const json = await data.json()
    res.status(data.status).json(json)
}
