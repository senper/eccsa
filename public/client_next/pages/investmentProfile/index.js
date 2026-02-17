import { useRouter } from "next/router";

export default function InvestmentProfileDefault() {
    const location = useRouter();
    location.push("/investmentProfile/ethiopia");
}