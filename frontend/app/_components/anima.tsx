"use client";
import { useCallback, useState } from "react";
import * as THREE from "three";
export default function Anima<T extends HTMLElement | string>() {
  const [vantaEffect, setVantaEffect] = useState<any | null>(null);
  const setRef = useCallback((el: HTMLDivElement | null) => {
    if (!vantaEffect && el) {
      /** @ts-expect-error
       * Vanta don't have types definitions
       */
      import("vanta/src/vanta.dots").then((VANTA) => {
        const effect = VANTA.default({
          el: el,
          THREE: THREE,
          mouseControls: true,
          touchControls: true,
          gyroControls: false,
          minHeight: 200.0,
          minWidth: 200.0,
          scale: 1.0,
          scaleMobile: 1.0,
          color: 0x707070,
          backgroundColor: 0xffffff,
          size: 5.0,
          spacing: 33.0,
          showLines: false,
        });
        setVantaEffect(effect);
      });
    }

    return () => {
      /** @ts-expect-error
       * Vanta don't have types definitions
       */

      if (vantaEffect) vantaEffect.destroy();
    };
  }, []);
  return setRef;
}
