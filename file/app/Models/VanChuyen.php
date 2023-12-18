<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class VanChuyen extends Model
{
    protected $table = 'chitietdonhang';

    protected $fillable = [
        'donhang_id',
        'sach_id',
        'vpp_id',
        'soluong',
        'giasanpham',
        'thanhtien',
    ];

    public function donHang()
    {
        return $this->belongsTo(DonHang::class, 'donhang_id', 'id');
    }

    public function sach()
    {
        return $this->belongsTo(Sach::class, 'sach_id', 'id');
    }

    public function vanPhongPham()
    {
        return $this->belongsTo(VanPhongPham::class, 'vpp_id', 'id');
    }
}
