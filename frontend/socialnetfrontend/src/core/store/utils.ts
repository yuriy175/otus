export const delay2 = (ms: number) => {
    return new Promise(resolve => setTimeout(resolve, ms));
  }