'use client'
 
import { useState } from 'react'
 
export default function Counter() {
  const [count, setCount] = useState(0)
 
  return (
    <div>
      <h1 className="text-3xl font-bold underline">I'm a client component!</h1>
      <p className="text-3xl font-bold underline">You clicked {count} times</p>
      <button className="text-3xl font-bold underline" onClick={() => setCount(count + 1)}>Click me</button>
    </div>
  )
}