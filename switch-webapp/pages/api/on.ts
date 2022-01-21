// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'

type Data = {
  name: string
}

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Data>
) {
  if(req.query.code != process.env.code)
    return res.status(403).send({message: "invalidCode"} as any);
  
  fetch("http://192.168.10.193:5000/on").then(() => console.log("Click"))
  res.status(200).json({ name: 'On' })
}
