import { useRouter } from "next/router";

export default function LawsDefault() {
    const location = useRouter();
    location.push("/laws/trade");
}