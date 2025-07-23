declare module "vanta/src/vanta.dots" {
  import { Object3D } from "three";

  export type VantaEffectConfig = {
    el: HTMLElement | string;
    THREE?: any; // you can use typeof import("three") if you want stronger typing
    mouseControls?: boolean;
    touchControls?: boolean;
    gyroControls?: boolean;
    minHeight?: number;
    minWidth?: number;
    scale?: number;
    scaleMobile?: number;
    color?: number;
    backgroundColor?: number;
    size?: number;
    spacing?: number;
    showLines?: boolean;
    [key: string]: any; // fallback for extra unknown options
  };

  interface VantaEffect {
    destroy: () => void;
    // Add other lifecycle methods if needed
  }

  const vantaDots: (config: VantaEffectConfig) => VantaEffect;

  export default vantaDots;
}
