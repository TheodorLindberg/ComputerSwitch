// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'

type Request = {
  code: string
}
type Response = {
  valid: boolean,
}

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Response>
) {

  
  if(req.query.code == process.env.CODE) {
    res.setHeader("Set-Cookie", `code=${req.query.code}`)
    res.status(200).json({valid: true})
    
  } else {

    res.status(403).json({valid: false})
  }
}
